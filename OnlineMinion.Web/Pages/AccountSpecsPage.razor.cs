using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Components;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.AppMessaging;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.RestApi.Client.Requests;
using OnlineMinion.Web.Components;
using OnlineMinion.Web.Infrastructure;
using OnlineMinion.Web.Shared;

namespace OnlineMinion.Web.Pages;

public partial class AccountSpecsPage : ComponentWithCancellationToken
{
    private AccountSpecsPageEditor _editorRef = null!;
    private bool _isSubmitting;
    private ModalDialog _modalDelete = null!;
    private string? _modalDeleteTitle;
    private ModalDialog _modalUpsert = null!;
    private string? _modalUpsertTitle;
    private AccountSpecResp? _modelDelete;
    private BaseUpsertAccountSpecReqData? _modelUpsert;

    private PagingMetaInfo? _paging;
    private List<AccountSpecResp>? _vm;

    [Inject]
    public IMediator Mediator { get; set; } = default!;

    [Inject]
    public NavigationManager Navigation { get; set; } = default!;

    [Inject]
    public ILogger<AccountSpecsPage> Logger { get; set; } = default!;

    [Parameter]
    [SupplyParameterFromQuery]
    public int Page { get; set; }

    [Parameter]
    [SupplyParameterFromQuery]
    public int PageSize { get; set; }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        // This order ensures that incoming params are sett before they ar needed by data loading.
        await base.SetParametersAsync(parameters);
        await GetPageViewModel();
    }

    protected override void OnInitialized()
    {
        Page = Page == default ? PagingMetaInfo.DefaultCurrent : Page;
        PageSize = PageSize == default ? PagingMetaInfo.DefaultSize : PageSize;
        base.OnInitialized();
    }

    private async Task GetPageViewModel()
    {
        _vm = null;

        var result = await Mediator.Send(new GetAccountSpecsReq(Page, PageSize), CT);

        _paging = result.Paging;
        StateHasChanged();

        _vm = new();

        await foreach (var model in result.Result.WithCancellation(CT))
        {
            _vm.Add(model);
            StateHasChanged();
        }
    }

    private void PageChanged(int page)
    {
        var old = Page;

        Page = _paging?.SanitizePage(page) ?? PagingMetaInfo.First;
        if (Page == old)
        {
            return;
        }

        NavigateByPager();
    }

    private void PageSizeChanged(int size)
    {
        PageSize = size;
        Page = _paging?.GetNewCurrentBySize(PageSize) ?? PagingMetaInfo.First;

        NavigateByPager();
    }

    private void NavigateByPager(int? page = null) => Navigation.NavigateTo(
        Navigation.GetUriWithQueryParameters(
            new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase)
                { [nameof(Page)] = page ?? Page, [nameof(PageSize)] = PageSize, }
        )
    );

    private void OnAddHandler()
    {
        if (_modelUpsert is not CreateAccountSpecReq)
        {
            _modelUpsert = new CreateAccountSpecReq();
        }

        OpenModelForCreate();
    }

    private void OpenModelForCreate()
    {
        _modalUpsertTitle = "Add new Account Specification";
        _modalUpsert.Open();
    }

    private async Task OnEditHandler(int id)
    {
        // If the editing of same item is already in progress, then do nothing.
        if (_modelUpsert is UpdateAccountSpecReq cmd && cmd.Id == id)
        {
            OpenModalForUpdate(id);

            return;
        }

        if (await Mediator.Send(new GetAccountSpecByIdReq(id), CT) is { } model)
        {
            _modelUpsert = new UpdateAccountSpecReq(model.Id, model.Name, model.Group, model.Description);
            OpenModalForUpdate(id);
        }
        else
        {
            // TODO: Notify this info to user as well!
            Logger.LogWarning("Account Specification {Id} do not exist anymore in database", id);
        }
    }

    private void OpenModalForUpdate(int id)
    {
        _modalUpsertTitle = $"Edit Account Specification: id#{id}";
        _modalUpsert.Open();
    }

    private async Task OnValidSubmitHandler()
    {
        _isSubmitting = true;

        switch (_modelUpsert)
        {
            case null:
                break;
            case UpdateAccountSpecReq req:
                await HandleUpdateSubmit(req);
                break;
            case CreateAccountSpecReq req:
                await HandleCreateSubmit(req);
                break;
        }

        _isSubmitting = false;
    }

    private async Task HandleUpdateSubmit(UpdateAccountSpecReq request)
    {
        var result = await Mediator.Send(request, CT);

        result.Switch(
            _ =>
            {
                UpdateViewModelAfterEdit(request);
                ResetUpsertModal();
            },
            errors =>
            {
                _editorRef.SetServerValidationErrors(errors);

                if (errors.Exists(err => err.Type is ErrorType.NotFound))
                {
                    // TODO: implement UX considering last element on page so that users always sees correct page.
                    _vm!.Remove(_vm.Single(m => m.Id == request.Id));
                }

                //TODO handel all other errors
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


    private async Task HandleCreateSubmit(CreateAccountSpecReq req)
    {
        var result = await Mediator.Send(req, CT);

        await result.SwitchAsync(
            async _ =>
            {
                await NavigateToNewItemPage();
                ResetUpsertModal();
            },
            errors =>
            {
                _editorRef.SetServerValidationErrors(errors);

                //TODO handel all other errors

                return Task.CompletedTask;
            }
        );
    }

    private async Task NavigateToNewItemPage()
    {
        if (await Mediator.Send(new GetAccountSpecPageCountBySizeReq(PageSize), CT) is not { } pages)
        {
            Logger.LogWarning("Table pagination to new element's page skipped, didn't got paging metadata");

            return;
        }

        if (pages == Page)
        {
            await GetPageViewModel();
        }
        else
        {
            NavigateByPager(pages);
        }
    }

    private void ResetUpsertModal()
    {
        _modalUpsert.Close();
        _modelUpsert = null;
    }

    private void OnDeleteHandler(int id)
    {
        _modelDelete = _vm!.Single(m => m.Id == id);
        _modalDeleteTitle = $"Delete Account Specification: id#{id}";
        _modalDelete.Open();
    }

    private async Task OnDeleteSubmitHandler()
    {
        if (_modelDelete is null)
        {
            return;
        }

        var id = _modelDelete.Value.Id;

        _isSubmitting = true;
        var result = await Mediator.Send(new DeleteAccountSpecReq(id), CT);
        _isSubmitting = false;

        await result.SwitchFirstAsync(
            async _ =>
            {
                _modalDelete.Close();
                await HandlePageStateAfterDelete();
            },
            error =>
            {
                if (error.Type is ErrorType.NotFound)
                {
                    Logger.LogWarning("Account Specification '{Id}' do not exist anymore in database", id);
                }
                else
                {
                    Logger.LogError("Unexpected failure while trying to delete Account Specification '{Id}'", id);
                }

                return Task.CompletedTask;
            }
        );
    }

    private async Task HandlePageStateAfterDelete()
    {
        // Detect it was last item on page, or very last item of the whole resource.
        // Navigate only, when page has to be changed, get current page's items otherwise.
        if (_paging?.TotalItems > 1 && _vm?.Count == 1)
        {
            Page = _paging!.Value.Previous;
            NavigateByPager();
        }
        else
        {
            await GetPageViewModel();
        }

        _modelDelete = null;
    }
}
