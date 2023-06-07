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

        _modalUpsertTitle = "Add new Account Specification";
        _modalUpsert.Open();
    }

    private async Task OnEditHandler(int id)
    {
        if (_modelUpsert is UpdateAccountSpecReq cmd && cmd.Id == id)
        {
            OpenModalForEdit(id);

            return;
        }

        if (await Mediator.Send(new GetAccountSpecByIdReq(id), CT) is { } model)
        {
            _modelUpsert = new UpdateAccountSpecReq(model.Id, model.Name, model.Group, model.Description);
            OpenModalForEdit(id);
        }
        else
        {
            // TODO: Notify this info to user as well!
            Logger.LogWarning("Account Specification {Id} do not exist anymore in database", id);
        }
    }

    private void OpenModalForEdit(int id)
    {
        _modalUpsertTitle = $"Edit Account Specification: id#{id}";
        _modalUpsert.Open();
    }

    private async Task OnValidSubmitHandler()
    {
        if (_modelUpsert is null)
        {
            return;
        }

        _isSubmitting = true;
        var result = await Mediator.Send(_modelUpsert, CT) as IErrorOr;
        _isSubmitting = false;

        if (result!.IsError)
        {
            _editorRef.SetServerValidationErrors(result.Errors!);

            if (result.Errors!.Exists(err => err.Type is ErrorType.NotFound)
                && _modelUpsert is UpdateAccountSpecReq updateModel)
            {
                // TODO: implement UX considering last element on page so that users always sees correct page.
                _vm!.Remove(_vm.Single(m => m.Id == updateModel.Id));
            }

            //TODO handel all other errors

            return;
        }

        _modalUpsert.Close();

        switch (_modelUpsert)
        {
            case UpdateAccountSpecReq req:
                UpdateViewModel(req);
                break;
            case CreateAccountSpecReq:
                await NavigateToNewItemPage();
                break;
        }

        _modelUpsert = null;
    }

    private void UpdateViewModel(UpdateAccountSpecReq request)
    {
        var model = _vm!.Single(m => m.Id == request.Id);
        var clone = model with { Name = request.Name, Group = request.Group, Description = request.Description, };

        _vm![_vm.IndexOf(model)] = clone;
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

        if (result)
        {
            _modalDelete.Close();
            await HandlePageStateAfterDelete();
        }
        else
        {
            Logger.LogWarning("Account Specification {Id} do not exist anymore in database", id);
        }
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
