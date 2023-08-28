using ErrorOr;
using JetBrains.Annotations;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Transactions.Common;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Contracts.Transactions.Responses;
using OnlineMinion.Web.Components;
using OnlineMinion.Web.Pages.Base;

namespace OnlineMinion.Web.Pages;

[UsedImplicitly]
public partial class TransactionCreditsPage : BaseCRUDPage<TransactionCreditResp>
{
    private const string ModelTypeName = "Credit Transaction";

    private readonly IEnumerable<int> _pageSizeOptions = BasePagingParams.AllowedSizes;
    private UpsertEditorWrapper<BaseUpsertTransactionReqData> _editorRef = null!;
    private RadzenDataGridWrapper<TransactionCreditResp> _gridWrapperRef = null!;
    private BaseUpsertTransactionReqData? _modelUpsert;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
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

    private void OnEditHandler(TransactionCreditResp model)
    {
        // If the editing of same item is already in progress, then continue editing it.
        if (_modelUpsert is not UpdateTransactionCreditReq cmd || cmd.Id != model.Id)
        {
            _modelUpsert = (UpdateTransactionCreditReq)model;
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

    protected override TransactionCreditResp ConvertToModel(IUpdateCommand updateRequest) =>
        (TransactionCreditResp)(UpdateTransactionCreditReq)updateRequest;

    protected override IGetPagingInfoRequest PageCountRequestFactory(int pageSize) =>
        new GetTransactionCreditPagingMetaInfoReq(pageSize);

    private async Task OnDeleteHandler(TransactionCreditResp model)
    {
        var (id, date, _, subject, _, _, _, _) = model;
        var descriptorData = $"subject `{subject}`, at {date}";

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
