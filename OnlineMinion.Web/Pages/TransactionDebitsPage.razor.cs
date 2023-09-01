using System.Globalization;
using ErrorOr;
using JetBrains.Annotations;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Transactions.Debit;
using OnlineMinion.Contracts.Transactions.Debit.Requests;
using OnlineMinion.Contracts.Transactions.Debit.Responses;
using OnlineMinion.Web.Components;
using OnlineMinion.Web.Helpers;
using OnlineMinion.Web.Pages.Base;
using OnlineMinion.Web.Transaction.Debit;

namespace OnlineMinion.Web.Pages;

[UsedImplicitly]
public partial class TransactionDebitsPage : BaseCRUDPage<TransactionDebitListItem, TransactionDebitResp>
{
    private const string ModelTypeName = "Debit Transaction";

    private readonly IEnumerable<int> _pageSizeOptions = BasePagingParams.AllowedSizes;
    private UpsertEditorWrapper<BaseUpsertTransactionDebitReqData> _editorRef = null!;
    private RadzenDataGridWrapper<TransactionDebitListItem> _gridWrapperRef = null!;
    private BaseUpsertTransactionDebitReqData? _modelUpsert;

    private IList<PaymentSpecResp> PaymentViewModels { get; } = new List<PaymentSpecResp>();
    public IList<AccountSpecResp> AccountViewModels { get; } = new List<AccountSpecResp>();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        // Order! Related models data is used to create create grid data, during main models pulling from stream.
        await Task.WhenAll(
            LoadRelatedViewModelModelFromApi(PaymentViewModels),
            LoadRelatedViewModelModelFromApi(AccountViewModels)
        );
        await LoadViewModelFromApi(CurrentPage, CurrentPageSize, string.Empty, string.Empty);
    }

    private async Task LoadRelatedViewModelModelFromApi<TResponse>(IList<TResponse> targetList)
    {
        var rq = new BaseGetSomeModelsPagedReq<TResponse>();
        var result = await Sender.Send(rq, CT);

        // TODO: need separate request-response objects with needed data only.
        await result.SwitchFirstAsync(
            async models => await models.Result.PullItemsFromStream(targetList, CT),
            firstError =>
            {
                Logger.LogError(
                    "Unexpected error while getting {ModelName} list: {ErrorDescription}",
                    nameof(TResponse),
                    firstError.Description
                );

                return Task.CompletedTask;
            }
        );
    }

    private void OnAddHandler()
    {
        if (_modelUpsert is not CreateTransactionDebitReq)
        {
            _modelUpsert = new CreateTransactionDebitReq();
        }

        OpenEditorDialog($"Add new {ModelTypeName}");
    }

    private void OnEditHandler(TransactionDebitListItem model)
    {
        // If the editing of same item is already in progress, then continue editing it.
        if (_modelUpsert is not UpdateTransactionDebitReq cmd || cmd.Id != model.Id)
        {
            _modelUpsert = TransactionDebitListItem.ToUpdateRequest(model);
        }

        OpenEditorDialog($"Edit {ModelTypeName}: id #{model.Id.ToString(CultureInfo.InvariantCulture)}");
    }

    protected override ValueTask<bool> Validate() => _editorRef.ValidateEditorAsync();

    protected override void SetServerValidationErrors(IList<Error> errors) =>
        _editorRef.SetServerValidationErrors(errors);

    protected override void CloseEditorDialog()
    {
        base.CloseEditorDialog();
        _editorRef.ResetEditor();
        _modelUpsert = null;
    }

    protected override async ValueTask<bool> HandleUpsertSubmit() => _modelUpsert switch
    {
        UpdateTransactionDebitReq req => await HandleUpdateSubmit(req),
        CreateTransactionDebitReq req => await HandleCreateSubmit(req),
        null => throw new InvalidOperationException("Upsert model is null."),
        _ => throw new InvalidOperationException("Unknown model type."),
    };

    protected override TransactionDebitListItem ConvertRequestResponseToVM(TransactionDebitResp dto)
    {
        var paymentSpec = PaymentViewModels.Single(vm => vm.Id == dto.PaymentInstrumentId);
        var accountSpec = AccountViewModels.Single(vm => vm.Id == dto.AccountSpecId);

        return TransactionDebitListItem.FromResponseDto(dto, paymentSpec.Name, accountSpec.Name);
    }

    protected override TransactionDebitListItem ConvertUpdateRequestToVM(IUpdateCommand dto)
    {
        var rq = (UpdateTransactionDebitReq)dto;
        var paymentSpec = PaymentViewModels.Single(vm => vm.Id == rq.PaymentInstrumentId);
        var accountSpec = AccountViewModels.Single(vm => vm.Id == rq.AccountSpecId);

        return TransactionDebitListItem.FromUpdateRequest(
            (UpdateTransactionDebitReq)dto,
            paymentSpec.Name,
            accountSpec.Name
        );
    }

    protected override IGetPagingInfoRequest PageCountRequestFactory(int pageSize) =>
        new GetTransactionDebitPagingMetaInfoReq(pageSize);

    private async Task OnDeleteHandler(TransactionDebitListItem model)
    {
        var descriptorData = $"subject `{model.Subject}`, at {model.Date.ToString(CultureInfo.CurrentCulture)}";

        if (!await GetUserConfirmation(descriptorData, ModelTypeName, null))
        {
            return;
        }

        SC.IsBusy = true;
        await DeleteModelFromApi(new DeleteTransactionDebitReq(model.Id), ModelTypeName);
        SC.IsBusy = false;
    }

    protected override async Task AfterSuccessfulChange(int apiPage) =>
        await _gridWrapperRef.DataGridRef.GoToPage(ToGridPage(apiPage), true);
}
