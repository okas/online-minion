using ErrorOr;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.Contracts.Common.Requests;
using OnlineMinion.RestApi.Client.AccountSpec.Requests;
using OnlineMinion.Web.Components;
using OnlineMinion.Web.Pages.Base;

namespace OnlineMinion.Web.Pages;

[UsedImplicitly]
public partial class AccountSpecsPage : BaseCRUDPage<AccountSpecResp>
{
    private const string ModelTypeName = "Account Specification";

    private readonly IEnumerable<int> _pageSizeOptions = BasePagingParams.AllowedSizes;
    private AccountSpecsEditor _editorRef = null!;
    private RadzenDataGridWrapper<AccountSpecResp> _gridWrapperRef = null!;
    private BaseUpsertAccountSpecReqData? _modelUpsert;

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

        OpenEditorDialog($"Add new {ModelTypeName}");
    }

    private void OnEditHandler(AccountSpecResp model)
    {
        // If the editing of same item is already in progress, then continue editing it.
        if (_modelUpsert is not UpdateAccountSpecReq cmd || cmd.Id != model.Id)
        {
            _modelUpsert = new UpdateAccountSpecReq(model.Id, model.Name, model.Group, model.Description);
        }

        OpenEditorDialog($"Edit {ModelTypeName}: id #{model.Id}");
    }

    protected override ValueTask<bool> Validate() => _editorRef.WrapperRef.ValidateEditorAsync();

    protected override void SetServerValidationErrors(IList<Error> errors) =>
        _editorRef.WrapperRef.SetServerValidationErrors(errors);

    protected override void CloseEditorDialog()
    {
        _modelUpsert = null;
        _editorRef.WrapperRef.ResetEditor();
        base.CloseEditorDialog();
    }

    protected override async ValueTask<bool> HandleUpsertSubmit() => _modelUpsert switch
    {
        UpdateAccountSpecReq req => await HandleUpdateSubmit(req),
        CreateAccountSpecReq req => await HandleCreateSubmit(req),
        null => throw new InvalidOperationException("Upsert model is null."),
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
        var (id, name, _, _) = model;

        if (!await GetUserConfirmation(name, ModelTypeName))
        {
            return;
        }

        SC.IsBusy = true;
        await DeleteModelFromApi(new DeleteAccountSpecReq(id), ModelTypeName);
        SC.IsBusy = false;
    }

    protected override async ValueTask AfterSuccessfulChange(int apiPage) =>
        await _gridWrapperRef.DataGridRef.GoToPage(ToGridPage(apiPage), true);
}
