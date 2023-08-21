using ErrorOr;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.AppMessaging;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.RestApi.Client.Requests;
using OnlineMinion.Web.Components;
using OnlineMinion.Web.Pages.Base;

namespace OnlineMinion.Web.Pages;

[UsedImplicitly]
public partial class AccountSpecsPage : BaseCRUDPage<AccountSpecResp>
{
    private readonly IEnumerable<int> _pageSizeOptions = BasePagingParams.AllowedSizes;
    private AccountSpecsEditor _editorRef = null!;
    private BaseUpsertAccountSpecReqData? _modelUpsert;
    private RadzenDataGridWrapper<AccountSpecResp> _gridWrapperRef = null!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await LoadViewModelFromApi(CurrentPage, CurrentPageSize);
    }

    private void OnAddHandler()
    {
        if (_modelUpsert is not CreateAccountSpecReq)
        {
            _modelUpsert = new CreateAccountSpecReq();
        }

        OpenEditorDialog("Add new Account Specification");
    }

    private void OnEditHandler(AccountSpecResp model)
    {
        // If the editing of same item is already in progress, then continue editing it.
        if (_modelUpsert is not UpdateAccountSpecReq cmd || cmd.Id != model.Id)
        {
            _modelUpsert = new UpdateAccountSpecReq(model.Id, model.Name, model.Group, model.Description);
        }

        OpenEditorDialog($"Edit Account Specification: id #{model.Id}");
    }

    protected override ValueTask<bool> Validate() => _editorRef.ValidateEditorAsync();

    protected override void SetServerValidationErrors(IList<Error> errors) =>
        _editorRef.SetServerValidationErrors(errors);

    protected override void CloseEditorDialog()
    {
        _modelUpsert = null;
        _editorRef.ResetEditor();
        base.CloseEditorDialog();
    }

    protected override async ValueTask<bool> HandleUpsertSubmit() => _modelUpsert switch
    {
        UpdateAccountSpecReq req => await HandleUpdateSubmit(req),
        CreateAccountSpecReq req => await HandleCreateSubmit(req),
        _ => throw new InvalidOperationException("Unknown model type."),
    };

    protected override AccountSpecResp ApplyUpdateToModel(AccountSpecResp model, IUpdateCommand updateRequest)
    {
        var request = (UpdateAccountSpecReq)updateRequest;

        return model with
        {
            Name = request.Name,
            Group = request.Group,
            Description = request.Description,
        };
    }

    protected override IRequest<int?> PageCountRequestFactory(int pageSize) =>
        new GetAccountSpecPageCountBySizeReq(pageSize);

    private async Task OnDeleteHandler(AccountSpecResp model)
    {
        const string modelType = "Account Specification";

        var (id, name, _, _) = model;

        if (!await GetUserConfirmation(name, modelType))
        {
            return;
        }

        SC.IsBusy = true;
        await DeleteModelFromApi(new DeleteAccountSpecReq(id), modelType);
        SC.IsBusy = false;
    }

    protected override async ValueTask AfterSuccessfulChange(int apiPage) =>
        await _gridWrapperRef.DataGridRef.GoToPage(ToGridPage(apiPage), true);
}
