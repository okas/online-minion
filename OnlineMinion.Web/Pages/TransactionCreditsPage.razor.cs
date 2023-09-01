using System.Globalization;
using ErrorOr;
using JetBrains.Annotations;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Transactions;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Contracts.Transactions.Credit.Responses;
using OnlineMinion.Web.Components;
using OnlineMinion.Web.Helpers;
using OnlineMinion.Web.Pages.Base;
using OnlineMinion.Web.Transaction.Credit;

namespace OnlineMinion.Web.Pages;

[UsedImplicitly]
public partial class TransactionCreditsPage : BaseCRUDPage<TransactionCreditListItem, TransactionCreditResp>
{
    private const string ModelTypeName = "Credit Transaction";

    private readonly IEnumerable<int> _pageSizeOptions = BasePagingParams.AllowedSizes;
    private UpsertEditorWrapper<BaseUpsertTransactionReqData> _editorRef = null!;
    private RadzenDataGridWrapper<TransactionCreditListItem> _gridWrapperRef = null!;
    private BaseUpsertTransactionReqData? _modelUpsert;

    private IList<PaymentSpecResp> RelatedViewModels { get; } = new List<PaymentSpecResp>();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        // Order! Related models data is used to create create list, during main models pulling from stream.
        await LoadRelatedViewModelModelFromApi();
        await LoadViewModelFromApi(CurrentPage, CurrentPageSize, string.Empty, string.Empty);
    }

    private async Task LoadRelatedViewModelModelFromApi()
    {
        var rq = new BaseGetSomeModelsPagedReq<PaymentSpecResp>();
        var result = await Sender.Send(rq, CT);

        // TODO: need separate request-response objects with needed data only.
        await result.SwitchFirstAsync(
            async models => await models.Result.PullItemsFromStream(RelatedViewModels, CT),
            firstError =>
            {
                Logger.LogError(
                    "Unexpected error while getting {ModelName} list: {ErrorDescription}",
                    nameof(PaymentSpecResp),
                    firstError.Description
                );

                return Task.CompletedTask;
            }
        );
    }

    private void OnAddHandler()
    {
        if (_modelUpsert is not CreateTransactionCreditReq)
        {
            _modelUpsert = new CreateTransactionCreditReq();
        }

        OpenEditorDialog($"Add new {ModelTypeName}");
    }

    private void OnEditHandler(TransactionCreditListItem model)
    {
        // If the editing of same item is already in progress, then continue editing it.
        if (_modelUpsert is not UpdateTransactionCreditReq cmd || cmd.Id != model.Id)
        {
            _modelUpsert = TransactionCreditListItem.ToUpdateRequest(model);
        }

        OpenEditorDialog($"Edit {ModelTypeName}: id #{model.Id}");
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
        UpdateTransactionCreditReq req => await HandleUpdateSubmit(req),
        CreateTransactionCreditReq req => await HandleCreateSubmit(req),
        null => throw new InvalidOperationException("Upsert model is null."),
        _ => throw new InvalidOperationException("Unknown model type."),
    };

    protected override TransactionCreditListItem ConvertRequestResponseToVM(TransactionCreditResp dto)
    {
        var paymentSpec = RelatedViewModels.Single(vm => vm.Id == dto.PaymentInstrumentId);

        return TransactionCreditListItem.FromResponseDto(dto, paymentSpec.Name);
    }

    protected override TransactionCreditListItem ConvertUpdateRequestToVM(IUpdateCommand dto)
    {
        var rq = (UpdateTransactionCreditReq)dto;
        var paymentSpec = RelatedViewModels.Single(vm => vm.Id == rq.PaymentInstrumentId);

        return TransactionCreditListItem.FromUpdateRequest((UpdateTransactionCreditReq)dto, paymentSpec.Name);
    }

    protected override IGetPagingInfoRequest PageCountRequestFactory(int pageSize) =>
        new GetTransactionCreditPagingMetaInfoReq(pageSize);

    private async Task OnDeleteHandler(TransactionCreditListItem model)
    {
        var (id, date, _, subject, _, _, _, _) = model;
        var descriptorData = $"subject `{subject}`, at {date.ToString(CultureInfo.CurrentCulture)}";

        if (!await GetUserConfirmation(descriptorData, ModelTypeName, null))
        {
            return;
        }

        SC.IsBusy = true;
        await DeleteModelFromApi(new DeleteTransactionCreditReq(id), ModelTypeName);
        SC.IsBusy = false;
    }

    protected override async Task AfterSuccessfulChange(int apiPage) =>
        await _gridWrapperRef.DataGridRef.GoToPage(ToGridPage(apiPage), true);
}
