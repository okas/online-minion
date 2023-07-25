using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Components;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.AppMessaging;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.RestApi.Client.Requests;
using OnlineMinion.Web.Components;
using OnlineMinion.Web.Helpers;
using OnlineMinion.Web.Pages.Base;
using Radzen;
using Radzen.Blazor;

namespace OnlineMinion.Web.Pages;

// TODO: Restore functionality where on add it can jump to last or page of new item.
// TODO: Also restore funtionality to reload given page on delete.
public partial class AccountSpecsPage : ComponentWithCancellationToken
{
    private readonly IEnumerable<int> _pageSizeOptions;
    private readonly List<AccountSpecResp> _vm;
    private int _currentPage;
    private int _currentPageSize;
    private RadzenDataGrid<AccountSpecResp> _dataGridRef = null!;
    private AccountSpecsEditor _editorRef = null!;
    private BaseUpsertAccountSpecReqData? _modelUpsert;
    private int _totalItemsCount;

    public AccountSpecsPage()
    {
        _currentPage = BasePagingParams.FirstPage;
        _currentPageSize = BasePagingParams.DefaultSize;
        _vm = new(_currentPageSize);
        _pageSizeOptions = BasePagingParams.AllowedSizes;
    }

    [Inject]
    public ISender Sender { get; set; } = default!;

    [Inject]
    public DialogService DialogService { get; set; } = default!;

    [Inject]
    public StateContainer SC { get; set; } = default!;

    [Inject]
    public ILogger<AccountSpecsPage> Logger { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await LoadViewModelFromApi(_currentPage, _currentPageSize);
    }

    private async Task OnLoadDataHandler(LoadDataArgs args)
    {
        var size = args.Top.GetValueOrDefault(BasePagingParams.DefaultSize);
        var offset = args.Skip.GetValueOrDefault();
        var page = offset > 0
            ? (int)Math.Ceiling((decimal)offset / size)
            : 1;

        await LoadViewModelFromApi(page, size, args.Filter, args.OrderBy);
    }

    /// <summary>
    ///     Using Dynamic LINQ expressions sends filter, sort and paging requests to API.
    ///     <a href="https://dynamic-linq.net/expression-language">Dynamic LINQ</a>
    /// </summary>
    /// <param name="page" />
    /// <param name="size" />
    /// <param name="filterExpression">Filtering expression, multi or single property.</param>
    /// <param name="sortExpression">Sorting expression, multi or single property.</param>
    private async Task LoadViewModelFromApi(
        int     page,
        int     size,
        string? filterExpression = default,
        string? sortExpression   = default
    )
    {
        SC.IsBusy = true;

        var result = await Sender.Send(new GetAccountSpecsReq(filterExpression, sortExpression, page, size), CT);

        _totalItemsCount = result.Paging.Rows;
        _vm.Clear();
        await result.Result.PullItemsFromStream(_vm, StateHasChanged, CT);

        SC.IsBusy = false;
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
                HandlePageStateAfterUpdate(request);

                return true;
            },
            errors =>
            {
                _editorRef.SetServerValidationErrors(errors);
                // It can happen, it is not unexpected error per se.
                if (errors.Exists(err => err.Type is ErrorType.NotFound))
                {
                    _dataGridRef.GoToPage(ToGridPage(_currentPage), true);
                }

                //TODO handel all other errors
                return false;
            }
        );
    }

    private void HandlePageStateAfterUpdate(UpdateAccountSpecReq request)
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

        return await result.MatchAsync(
            async _ =>
            {
                await HandlePageStateAfterCreate();
                return true;
            },
            errors =>
            {
                _editorRef.SetServerValidationErrors(errors);

                //TODO handel all other errors

                return Task.FromResult(false);
            }
        );
    }

    private async Task HandlePageStateAfterCreate()
    {
        if (await Sender.Send(new GetAccountSpecPageCountBySizeReq(_currentPageSize), CT) is { } pagesCount)
        {
            var lastPageAfterCreate = _currentPage == pagesCount
                ? _currentPage
                : pagesCount;

            await _dataGridRef.GoToPage(ToGridPage(lastPageAfterCreate), true);

            return;
        }

        Logger.LogWarning("Table pagination to new element's page skipped, didn't got paging metadata");
    }

    private async Task OnDeleteHandler(AccountSpecResp model)
    {
        var (id, name, _, _) = model;

        if (!await GetUserConfirmation(name))
        {
            return;
        }

        SC.IsBusy = true;
        await DeleteModelFromApi(id);
        SC.IsBusy = false;
    }

    private async Task<bool> GetUserConfirmation(string modelName) =>
        await DialogService.Confirm(
            $"Are you sure to delete Account spec with name <em>{modelName}</em>?",
            "Confirm deletion",
            new()
            {
                OkButtonText = "Confirm",
                CancelButtonText = "Cancel",
                Draggable = true,
                CloseDialogOnOverlayClick = true,
            }
        ) ?? false;

    private async Task<bool> DeleteModelFromApi(int id)
    {
        var result = await Sender.Send(new DeleteAccountSpecReq(id), CT);

        return await result.MatchFirstAsync(
            async _ =>
            {
                await HandlePageStateAfterDelete();

                return true;
            },
            error =>
            {
                HandleApiDeleteErrors(id, error);

                return Task.FromResult(false);
            }
        );
    }

    private async Task HandlePageStateAfterDelete()
    {
        var visiblePageAfterDelete = _vm.Count == 1
            ? _currentPage - 1
            : _currentPage;

        await _dataGridRef.GoToPage(ToGridPage(visiblePageAfterDelete), true);
    }

    private void HandleApiDeleteErrors(int id, Error error)
    {
        if (error.Type is ErrorType.NotFound)
        {
            Logger.LogWarning("Account Specification '{Id}' do not exist anymore in database", id);
        }
        else
        {
            Logger.LogError("Unexpected failure while trying to delete Account Specification '{Id}'", id);
        }
    }

    private void PagerChangeHandler(PagerEventArgs changeData)
    {
        // Grid page is 0-based;
        _currentPage = ToApiPage(changeData.PageIndex);
        _currentPageSize = changeData.Top;
    }

    private static int ToGridPage(int apiPage) => apiPage - 1;

    private static int ToApiPage(int gridPage) => gridPage + 1;
}
