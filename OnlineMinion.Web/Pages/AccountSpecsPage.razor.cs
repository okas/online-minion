using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Components;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.AppMessaging;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Web.Components;
using OnlineMinion.Web.Helpers;
using OnlineMinion.Web.Pages.Base;
using Radzen;

namespace OnlineMinion.Web.Pages;

// TODO: Restore functionality where on add it can jump to last or page of new item.
// TODO: Also restore funtionality to reload given page on delete.
public partial class AccountSpecsPage : ComponentWithCancellationToken
{
    private readonly IEnumerable<int> _pageSizeOptions;
    private readonly List<AccountSpecResp> _vm;
    private AccountSpecsEditor _editorRef = null!;
    private bool _isLoadingVm;
    private BaseUpsertAccountSpecReqData? _modelUpsert;
    private int _totalItemsCount;

    public AccountSpecsPage()
    {
        _vm = new(PagingMetaInfo.DefaultSize);
        _pageSizeOptions = new[] { 5, 10, 20, 50, 100, };
    }

    [Inject]
    public ISender Sender { get; set; } = default!;

    [Inject]
    public NavigationManager Navigation { get; set; } = default!;

    [Inject]
    public DialogService DialogService { get; set; } = default!;

    [Inject]
    public StateContainer SC { get; set; } = default!;

    [Inject]
    public ILogger<AccountSpecsPage> Logger { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await GetPageViewModel();
    }

    private async Task OnLoadDataHandler(LoadDataArgs args)
    {
        var pageSize = args.Top.GetValueOrDefault(args.Top.GetValueOrDefault());
        var pageNumber = (int)Math.Ceiling((decimal)(args.Skip.GetValueOrDefault() + 1) / pageSize);

        await GetPageViewModel(pageNumber, pageSize);
    }

    private async Task GetPageViewModel(
        int page = PagingMetaInfo.First,
        int size = PagingMetaInfo.DefaultSize
    )
    {
        _isLoadingVm = true;
        StateHasChanged();

        var result = await Sender.Send(new GetAccountSpecsReq(page, size), CT);

        _totalItemsCount = result.Paging.TotalItems;
        _vm.Clear();
        await result.Result.PullItemsFromStream(_vm, StateHasChanged, CT);

        _isLoadingVm = false;
        StateHasChanged();
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

    private void OpenEditorDialog(string title) => DialogService.Open(
        title,
        _ => RenderEditorComponent(),
        new() { Draggable = true, }
    );

    private void CloseEditorDialog()
    {
        _modelUpsert = null;
        _editorRef.ResetEditor();
        DialogService.Close();
    }

    private async Task OnUpsertSubmitHandler()
    {
        SC.IsBusy = true;

        if (!await _editorRef.ValidateEditorAsync())
        {
            SC.IsBusy = false;

            return;
        }

        var succeed = _modelUpsert switch
        {
            UpdateAccountSpecReq req => await HandleUpdateSubmit(req),
            CreateAccountSpecReq req => await HandleCreateSubmit(req),
            _ => throw new InvalidOperationException("Unknown model type."),
        };

        SC.IsBusy = false;

        if (succeed)
        {
            CloseEditorDialog();
        }
    }

    private async Task<bool> HandleUpdateSubmit(UpdateAccountSpecReq request)
    {
        var result = await Sender.Send(request, CT);

        return result.Match(
            _ =>
            {
                UpdateViewModelAfterEdit(request);
                return true;
            },
            errors =>
            {
                _editorRef.SetServerValidationErrors(errors);
                // It can happen, it is not unexpected error per se.
                if (errors.Exists(err => err.Type is ErrorType.NotFound))
                {
                    // TODO: implement UX considering last element on page so that users always sees correct page.
                    _vm!.Remove(_vm.Single(m => m.Id == request.Id));
                }

                //TODO handel all other errors
                return false;
            }
        );
    }

    private void UpdateViewModelAfterEdit(UpdateAccountSpecReq request)
    {
        var model = _vm!.Single(m => m.Id == request.Id);
        var clone = model with
        {
            Name = request.Name, Group = request.Group, Description = request.Description,
        };

        _vm![_vm.IndexOf(model)] = clone;
    }

    private async Task<bool> HandleCreateSubmit(CreateAccountSpecReq req)
    {
        var result = await Sender.Send(req, CT);

        return result.Match(
            _ => true,
            errors =>
            {
                _editorRef.SetServerValidationErrors(errors);

                //TODO handel all other errors

                return false;
            }
        );
    }


    private async Task OnDeleteHandler(AccountSpecResp model)
    {
        var hasConfirmed = await DialogService.Confirm(
            $"Are you sure to delete Account spec with name <em>{model.Name}</em>?",
            "Confirm deletion",
            new()
            {
                OkButtonText = "Confirm",
                CancelButtonText = "Cancel",
                Draggable = true,
                CloseDialogOnOverlayClick = true,
            }
        );

        if (hasConfirmed is true)
        {
            await OnDeleteConfirmHandler(model.Id);
        }
    }

    private async Task OnDeleteConfirmHandler(int modelId)
    {
        SC.IsBusy = true;

        var result = await Sender.Send(new DeleteAccountSpecReq(modelId), CT);
        await result.SwitchFirstAsync(
            async _ =>
            {
                // await HandlePageStateAfterDelete();
            },
            error =>
            {
                if (error.Type is ErrorType.NotFound)
                {
                    Logger.LogWarning("Account Specification '{Id}' do not exist anymore in database", modelId);
                }
                else
                {
                    Logger.LogError("Unexpected failure while trying to delete Account Specification '{Id}'", modelId);
                }

                return Task.CompletedTask;
            }
        );

        SC.IsBusy = false;
    }
}
