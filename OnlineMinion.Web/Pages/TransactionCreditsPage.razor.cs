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

    private List<PaymentSpecDescriptorResp> PaymentDescriptorViewModels { get; } = new();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        // Order! Related models data is used to create create list, during main models pulling from stream.
        await LoadDescriptorViewModelsFromApi(PaymentDescriptorViewModels);
        await LoadViewModelFromApi(CurrentPage, CurrentPageSize, string.Empty, string.Empty);
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
        UpdateTransactionCreditReq req => await HandleUpdateSubmit(req),
        CreateTransactionCreditReq req => await HandleCreateSubmit(req),
        null => throw new InvalidOperationException("Upsert model is null."),
        _ => throw new InvalidOperationException("Unknown model type."),
    };

    protected override TransactionCreditListItem ConvertRequestResponseToVM(TransactionCreditResp dto)
    {
        var paymentSpec = GetById(PaymentDescriptorViewModels, dto.PaymentInstrumentId);

        return TransactionCreditListItem.FromResponseDto(dto, paymentSpec);
    }

    protected override TransactionCreditListItem ConvertUpdateRequestToVM(IUpdateCommand dto)
    {
        var rq = (UpdateTransactionCreditReq)dto;
        var paymentSpec = GetById(PaymentDescriptorViewModels, rq.PaymentInstrumentId);

        return TransactionCreditListItem.FromUpdateRequest((UpdateTransactionCreditReq)dto, paymentSpec);
    }

    protected override IGetPagingInfoRequest PageCountRequestFactory(int pageSize) =>
        new GetTransactionCreditPagingMetaInfoReq(pageSize);

    private async Task OnDeleteHandler(TransactionCreditListItem model)
    {
        var descriptorData = $"subject `{model.Subject}`, at {model.Date.ToString(CultureInfo.CurrentCulture)}";

        if (!await GetUserConfirmation(descriptorData, ModelTypeName, null))
        {
            return;
        }

        SC.IsBusy = true;
        await DeleteModelFromApi(new DeleteTransactionCreditReq(model.Id), ModelTypeName);
        SC.IsBusy = false;
    }

    protected override async Task AfterSuccessfulChange(int apiPage) =>
        await _gridWrapperRef.DataGridRef.GoToPage(ToGridPage(apiPage), true);
}
