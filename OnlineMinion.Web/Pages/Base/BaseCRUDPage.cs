using System.Globalization;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Components;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Shared.Responses;
using OnlineMinion.Web.Components;
using OnlineMinion.Web.Helpers;
using Radzen;

namespace OnlineMinion.Web.Pages.Base;

/// <summary>Generic CRUD page for Radzen DataGrid and Editor components.</summary>
/// <remarks>
///     Has few abstract and virtual methods that are needed to provide page-specific object conversions and
///     behaviors.
/// </remarks>
/// <typeparam name="TVModel">It is the main type that is listed on data grid.</typeparam>
/// <typeparam name="TResponse">
///     API returned type. It can be different than <typeparamref name="TVModel" />, but it must be
///     specified.
/// </typeparam>
/// <typeparam name="TBaseUpsert"><b>class</b> constrained, because it is used in form, must be mutable.</typeparam>
public abstract class BaseCRUDPage<TVModel, TResponse, TBaseUpsert> : ComponentWithCancellationToken
    where TVModel : IHasIntId
    where TResponse : IHasIntId
    where TBaseUpsert : class
{
    private readonly string _respModelName;

    protected UpsertEditorWrapper<TBaseUpsert> EditorWrapperRef = null!;
    protected RadzenDataGridWrapper<TVModel> GridWrapperRef = null!;
    protected TBaseUpsert? UpsertVM;

    protected BaseCRUDPage()
    {
        _respModelName = typeof(TResponse).Name;
        PageSizeOptions = BasePagingParams.AllowedSizes;
        CurrentPage = BasePagingParams.FirstPage;
        CurrentPageSize = BasePagingParams.DefaultSize;
        ViewModels = new List<TVModel>(CurrentPageSize);
    }

    protected IEnumerable<int> PageSizeOptions { get; private set; }

    protected int CurrentPage { get; private set; }

    protected int CurrentPageSize { get; private set; }

    protected int TotalItemsCount { get; private set; }

    protected IList<TVModel> ViewModels { get; }

    [Inject]
    public ISender Sender { get; set; } = default!;

    [Inject]
    public StateContainer SC { get; set; } = default!;

    [Inject]
    public DialogService DialogService { get; set; } = default!;

    [Inject]
    public ILogger<BaseCRUDPage<TVModel, TResponse, TBaseUpsert>> Logger { get; set; } = default!;

    protected abstract string ModelTypeName { get; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        SC.IsBusy = true;

        // Order! Dependent models data is used to create create grid data, during main models pulling from stream.
        await RunDependencyLoadingAsync();
        await LoadModelsFromApiAsync(CurrentPage, CurrentPageSize, string.Empty, string.Empty);

        SC.IsBusy = false;
    }

    protected virtual Task RunDependencyLoadingAsync() => Task.CompletedTask;

    protected async Task OnLoadDataHandlerAsync(LoadDataArgs args)
    {
        SC.IsBusy = true;

        var size = args.Top.GetValueOrDefault(BasePagingParams.DefaultSize);
        var offset = args.Skip.GetValueOrDefault();
        var page = (int)Math.Floor((decimal)offset / size) + 1;

        await LoadModelsFromApiAsync(page, size, args.Filter, args.OrderBy);

        SC.IsBusy = false;
    }

    /// <summary>
    ///     Using Dynamic LINQ expressions sends filter, sort and paging requests to API.
    ///     <a href="https://dynamic-linq.net/expression-language">Dynamic LINQ</a>
    /// </summary>
    /// <param name="page" />
    /// <param name="size" />
    /// <param name="filterExpression">Filtering expression, multi or single property.</param>
    /// <param name="sortExpression">Sorting expression, multi or single property.</param>
    /// <remarks>
    ///     Pulling data from stream uses "afterEachPull" action to run <see cref="ComponentBase.StateHasChanged" /> to update
    ///     view as items
    ///     are stored into list.<br />
    ///     Pulling is done in <see cref="OnApiLoadItemsVMsSuccessAsync" />.
    /// </remarks>
    private async Task LoadModelsFromApiAsync(int page, int size, string filterExpression, string sortExpression)
    {
        LogOnViewModelDataLoad(page, size, filterExpression, sortExpression);

        var rq = new GetSomeModelsPagedReq<TResponse>(filterExpression, sortExpression, page, size);
        var result = await Sender.Send(rq, CT);

        await result.SwitchFirstAsync(
            OnApiLoadItemsVMsSuccessAsync,
            firstError =>
            {
                LogErrorOnViewModelDataApiLoad(firstError);

                return Task.CompletedTask;
            }
        );
    }

    private async Task OnApiLoadItemsVMsSuccessAsync(PagedStreamResult<TResponse> pagedStreamResult)
    {
        TotalItemsCount = pagedStreamResult.Paging.Rows;
        ViewModels.Clear();
        await pagedStreamResult.StreamResult.PullItemsFromStream(ItemVMPullAction, CT, StateHasChanged);
    }

    private void ItemVMPullAction(TResponse vm) => ViewModels.Add(ConvertReqResponseToVM(vm));

    protected abstract TVModel ConvertReqResponseToVM(TResponse dto);

    /// <summary>
    ///     Takes in request performs query and pulls data from stream using provided "pull action".<br />
    ///     Taking in action instead of list or returning data allows different types for response and view model.
    ///     <remarks>
    ///         Unlike pulling data in <see cref="LoadModelsFromApiAsync" /> workflow, this method does not use
    ///         "afterEachPull" action.
    ///     </remarks>
    /// </summary>
    /// <param name="rq">
    ///     Request used to query data from API.<br />
    ///     Its type ensures, that request comes in as async enumerable stream.
    ///     Otherwise it is up to this request and it's handler what and how is  retrieved.
    /// </param>
    /// <param name="pullAction">
    ///     Action that takes in instance of response and does something with it in its implementation.
    ///     Normally storing it to list, optionally converting it to other type. It can be as simple as `listInst.Add`.
    /// </param>
    /// <typeparam name="TDependentVmResponse">Type of API returned model data.</typeparam>
    protected async Task LoadDependencyFromApiAsync<TDependentVmResponse>(
        IGetStreamedRequest<TDependentVmResponse> rq,
        Action<TDependentVmResponse>              pullAction
    )
    {
        var result = await Sender.Send(rq, CT);

        // TODO: need separate rq-response objects with needed data only.
        await result.SwitchFirstAsync(
            async models => await models.PullItemsFromStream(pullAction, CT),
            firstError =>
            {
                LogErrorOnDescriptorViewModelDataApiLoad(firstError, typeof(TDependentVmResponse).Name);

                return Task.CompletedTask;
            }
        );
    }

    protected void OnAddHandler()
    {
        if (UpsertVM is not ICreateCommand)
        {
            UpsertVM = (TBaseUpsert)CreateVMFactory();
        }

        OpenEditorDialog($"Add new {ModelTypeName}");
    }

    protected abstract ICreateCommand CreateVMFactory();

    protected void OnEditHandler(TVModel vm)
    {
        // If the editing of same item is already in progress, then continue editing it.
        if (UpsertVM is not IUpdateCommand cmd || cmd.Id != vm.Id)
        {
            UpsertVM = (TBaseUpsert)UpdateVMFactory(vm);
        }

        var title = $"Edit {ModelTypeName}: id #{vm.Id.ToString(CultureInfo.InvariantCulture)}";
        OpenEditorDialog(title);
    }

    protected abstract IUpdateCommand UpdateVMFactory(TVModel vm);

    private void OpenEditorDialog(string title) => DialogService.Open(
        title,
        _ => RenderEditorComponent(),
        new() { Draggable = true, }
    );

    protected abstract RenderFragment RenderEditorComponent();

    protected async Task OnUpsertSubmitHandlerAsync()
    {
        SC.IsBusy = true;

        if (!await EditorWrapperRef.ValidateEditorAsync())
        {
            SC.IsBusy = false;

            return;
        }

        var succeed = await HandleUpsertSubmit();

        SC.IsBusy = false;

        if (succeed)
        {
            CloseEditorDialog();
        }
    }

    private ValueTask<bool> HandleUpsertSubmit() => UpsertVM switch
    {
        IUpdateCommand reqOrVM => SubmitUpdateCommandAsync(reqOrVM),
        ICreateCommand reqOrVM => SubmitCreateCommandAsync(reqOrVM),
        null => throw new InvalidOperationException("Upsert view model is null."),
        _ => throw new InvalidOperationException("Unknown view model type."),
    };

    private async ValueTask<bool> SubmitUpdateCommandAsync(IUpdateCommand reqOrVM)
    {
        var rq = ConvertUpdateVMToReq(reqOrVM);
        var result = await Sender.Send(rq, CT);

        return await result.MatchAsync(
            _ =>
            {
                OnApiUpdateSuccess(rq);

                return Task.FromResult(true);
            },
            async errors =>
            {
                await OnApiUpdateErrorAsync(errors);

                return false;
            }
        );
    }

    protected abstract IUpdateCommand ConvertUpdateVMToReq(IUpdateCommand reqOrVM);

    private void OnApiUpdateSuccess(IUpdateCommand rq)
    {
        var model = ViewModels.Single(m => m.Id == rq.Id);
        var clone = ConvertUpdateReqToVM(rq);
        ViewModels[ViewModels.IndexOf(model)] = clone;
    }

    private async Task OnApiUpdateErrorAsync(List<Error> errors)
    {
        SetServerValidationErrors(errors);

        // It can happen, it is not unexpected error per se.
        if (errors.Exists(err => err.Type is ErrorType.NotFound))
        {
            await ChangeGridPageAsync(CurrentPage);
        }

        //TODO handel all other errors
    }

    protected abstract TVModel ConvertUpdateReqToVM(IUpdateCommand dto);

    private async ValueTask<bool> SubmitCreateCommandAsync(ICreateCommand reqOrVM)
    {
        var rq = ConvertCreateVMToReq(reqOrVM);
        var result = await Sender.Send(rq, CT);

        return await result.MatchAsync(
            async _ =>
            {
                await OnApiCreateSuccessAsync();

                return true;
            },
            errors =>
            {
                OnApiCreateError(errors);

                return Task.FromResult(false);
            }
        );
    }

    protected abstract ICreateCommand ConvertCreateVMToReq(ICreateCommand reqOrVM);

    private void OnApiCreateError(IList<Error> errors)
    {
        SetServerValidationErrors(errors);

        //TODO handel all other errors
    }

    private async ValueTask OnApiCreateSuccessAsync()
    {
        var rq = PageCountRequestFactory(CurrentPageSize);
        var result = await Sender.Send(rq, CT);

        await result.SwitchFirstAsync(
            info => ChangeGridPageAsync(info.Pages),
            firstError =>
            {
                OnApiGetPagingInfoError(firstError);

                return Task.CompletedTask;
            }
        );
    }

    protected abstract IGetPagingInfoRequest PageCountRequestFactory(int pageSize);

    private void OnApiGetPagingInfoError(Error error)
    {
        if (error.Type is ErrorType.Failure)
        {
            LogWarningOnApiGetPagingInfo(error);

            // TODO: show error to user
            // TODO: "Table pagination to new element's page skipped"
        }
        else
        {
            LogErrorOnApiGetPagingInfo();
        }
    }


    private void SetServerValidationErrors(IList<Error> errors) => EditorWrapperRef.SetServerValidationErrors(errors);

    protected void CloseEditorDialog()
    {
        EditorWrapperRef.ResetEditor();
        DialogService.Close();
        UpsertVM = null;
    }

    protected async Task OnDeleteHandlerAsync(TVModel vm)
    {
        var descriptorData = GetDeleteMessageDescriptorData(vm);

        if (!await GetUserConfirmationAsync(descriptorData, null))
        {
            return;
        }

        SC.IsBusy = true;
        await DeleteModelFromApiAsync(vm);
        SC.IsBusy = false;
    }

    protected abstract string GetDeleteMessageDescriptorData(TVModel model);

    private async ValueTask<bool> DeleteModelFromApiAsync(TVModel vm)
    {
        var rq = DeleteCommandFactory(vm);
        var result = await Sender.Send(rq, CT);

        return await result.MatchFirstAsync(
            async _ =>
            {
                await OnApiDeleteSuccessAsync();

                return true;
            },
            error =>
            {
                OnApiDeleteError(vm.Id, error);

                return Task.FromResult(false);
            }
        );
    }

    protected abstract IDeleteByIdCommand DeleteCommandFactory(TVModel vm);

    private async ValueTask OnApiDeleteSuccessAsync()
    {
        var visiblePageAfterDelete = ViewModels.Count == 1
            ? CurrentPage - 1
            : CurrentPage;

        await ChangeGridPageAsync(visiblePageAfterDelete);
    }

    private void OnApiDeleteError(int id, Error error)
    {
        if (error.Type is ErrorType.NotFound)
        {
            LogWarningOnApiDelete(id);
        }
        else
        {
            LogErrorOnApiDelete(id);
        }
    }

    /// <summary>
    ///     Display confirmation dialog to user.
    /// </summary>
    /// <param name="descriptorData">Some details, like name or similar of model to present in dialog.</param>
    /// <param name="descriptorName">
    ///     Optional name for descriptor data in the sentence. Can be set tu null if
    ///     <paramref name="descriptorData" /> already has it.
    /// </param>
    private async ValueTask<bool> GetUserConfirmationAsync(string descriptorData, string? descriptorName = "name")
    {
        var name = string.IsNullOrWhiteSpace(descriptorName) ? string.Empty : $"{descriptorName.Trim()} ";
        var messageEng = $"Are you sure to delete {ModelTypeName} with {name}<em>{descriptorData}</em>?";

        const string title = "Confirm deletion";

        var options = new ConfirmOptions
        {
            OkButtonText = "Confirm",
            CancelButtonText = "Cancel",
            Draggable = true,
            CloseDialogOnOverlayClick = true,
        };

        return await DialogService.Confirm(messageEng, title, options) ?? false;
    }

    protected void PagerChangeHandler(PagerEventArgs changeData)
    {
        CurrentPage = ToApiPage(changeData.PageIndex);
        CurrentPageSize = changeData.Top;
    }

    private async Task ChangeGridPageAsync(int apiPage) =>
        await GridWrapperRef.DataGridRef.GoToPage(ToGridPage(apiPage), true);

    /// <summary>
    ///     Radzen DataGrid uses 0-based page index, but API uses 1-based page index.
    /// </summary>
    private static int ToGridPage(int apiPage) => apiPage - 1;

    /// <inheritdoc cref="ToGridPage" />
    private static int ToApiPage(int gridPage) => gridPage + 1;

    private void LogOnViewModelDataLoad(int page, int size, string filter, string sort) =>
        Logger.LogTrace(
            "Loading {ModelName} list from API: page={Page}, size={Size}, filter=`{Filter}`, sort=`{Sort}`",
            _respModelName,
            page,
            size,
            filter,
            sort
        );

    private void LogErrorOnViewModelDataApiLoad(Error error) =>
        Logger.LogError(
            "Unexpected error while getting {ModelName} list: {ErrorDescription}",
            _respModelName,
            error.Description
        );

    private void LogErrorOnDescriptorViewModelDataApiLoad(Error error, string modelName) =>
        Logger.LogError(
            "Unexpected error while querying descriptor view models data as `{ModelName}`: {ErrorDescription}",
            modelName,
            error.Description
        );

    private void LogWarningOnApiGetPagingInfo(Error error) =>
        Logger.LogWarning(
            "Table pagination to new element's page skipped: {ErrorDescription}",
            error.Description
        );

    private void LogErrorOnApiGetPagingInfo() =>
        Logger.LogError("Unexpected error while getting paging metadata to re-paginate table");

    private void LogWarningOnApiDelete(int id) =>
        Logger.LogWarning(
            "{ModelType} Id='{Id}' do not exist anymore in database",
            ModelTypeName,
            id
        );

    private void LogErrorOnApiDelete(int id) =>
        Logger.LogError(
            "Unexpected failure while trying to delete {ModelType} Id='{Id}'",
            ModelTypeName,
            id
        );
}
