using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Components;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Web.Helpers;
using Radzen;

namespace OnlineMinion.Web.Pages.Base;

public abstract class BaseCRUDPage<TModel> : ComponentWithCancellationToken
    where TModel : IHasIntId
{
    protected BaseCRUDPage()
    {
        CurrentPage = BasePagingParams.FirstPage;
        CurrentPageSize = BasePagingParams.DefaultSize;
        ViewModels = new List<TModel>(CurrentPageSize);
    }

    protected int CurrentPage { get; private set; }

    protected int CurrentPageSize { get; private set; }

    protected int TotalItemsCount { get; private set; }

    protected IList<TModel> ViewModels { get; }

    [Inject]
    public ISender Sender { get; set; } = default!;

    [Inject]
    public StateContainer SC { get; set; } = default!;

    [Inject]
    public DialogService DialogService { get; set; } = default!;

    [Inject]
    public ILogger<BaseCRUDPage<TModel>> Logger { get; set; } = default!;

    protected async Task OnLoadDataHandler(LoadDataArgs args)
    {
        var size = args.Top.GetValueOrDefault(BasePagingParams.DefaultSize);
        var offset = args.Skip.GetValueOrDefault();
        var page = (int)Math.Floor((decimal)offset / size) + 1;

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
    protected async ValueTask LoadViewModelFromApi(
        int     page,
        int     size,
        string? filterExpression = default,
        string? sortExpression   = default
    )
    {
        SC.IsBusy = true;

        var result = await Sender.Send(
            new BaseGetSomeModelsPagedReq<TModel>(filterExpression, sortExpression, page, size),
            CT
        );

        await result.SwitchFirstAsync(
            async pagedResult =>
            {
                TotalItemsCount = pagedResult.Paging.Rows;
                ViewModels.Clear();
                await pagedResult.Result.PullItemsFromStream(ViewModels, CT, StateHasChanged);
            },
            firstError =>
            {
                Logger.LogError(
                    "Unexpected error while getting {ModelName} list: {ErrorDescription}",
                    typeof(TModel).Name,
                    firstError.Description
                );

                return Task.CompletedTask;
            }
        );

        SC.IsBusy = false;
    }

    protected void OpenEditorDialog(string title) =>
        DialogService.Open(
            title,
            _ => RenderEditorComponent(),
            new() { Draggable = true, }
        );

    protected abstract RenderFragment RenderEditorComponent();

    protected async Task OnUpsertSubmitHandler()
    {
        SC.IsBusy = true;

        if (!await Validate())
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

    protected abstract ValueTask<bool> Validate();

    protected abstract ValueTask<bool> HandleUpsertSubmit();

    protected async Task<bool> HandleUpdateSubmit(IUpdateCommand request)
    {
        var result = await Sender.Send(request, CT);

        return await result.MatchAsync(
            _ =>
            {
                HandlePageStateAfterUpdate(request);

                return Task.FromResult(true);
            },
            async errors =>
            {
                SetServerValidationErrors(errors);
                // It can happen, it is not unexpected error per se.
                if (errors.Exists(err => err.Type is ErrorType.NotFound))
                {
                    await AfterSuccessfulChange(CurrentPage);
                }

                //TODO handel all other errors
                return false;
            }
        );
    }

    private void HandlePageStateAfterUpdate(IUpdateCommand rq)
    {
        var model = ViewModels.Single(m => m.Id == rq.Id);
        var clone = ApplyUpdateToModel(model, rq);
        ViewModels[ViewModels.IndexOf(model)] = clone;
    }

    protected abstract TModel ApplyUpdateToModel(TModel model, IUpdateCommand updateRequest);

    protected async Task<bool> HandleCreateSubmit(ICreateCommand rq)
    {
        var result = await Sender.Send(rq, CT);

        return await result.MatchAsync(
            async _ =>
            {
                var metaInfoRequest = PageCountRequestFactory(CurrentPageSize);
                await HandlePageStateAfterCreate(metaInfoRequest);

                return true;
            },
            errors =>
            {
                SetServerValidationErrors(errors);

                //TODO handel all other errors

                return Task.FromResult(false);
            }
        );
    }

    protected abstract IGetPagingInfoRequest PageCountRequestFactory(int pageSize);

    private async ValueTask HandlePageStateAfterCreate(IGetPagingInfoRequest metaInfoRequest)
    {
        var result = await Sender.Send(metaInfoRequest, CT);

        await result.SwitchFirstAsync(
            info => AfterSuccessfulChange(info.Pages),
            firstError =>
            {
                HandlePagingMetadataQueryErrors(firstError);

                return Task.CompletedTask;
            }
        );
    }

    private void HandlePagingMetadataQueryErrors(Error error)
    {
        if (error.Type is ErrorType.Failure)
        {
            Logger.LogWarning(
                "Table pagination to new element's page skipped: {ErrorDescription}",
                error.Description
            );
            // TODO: show error to user
            // TODO: "Table pagination to new element's page skipped"
        }
        else
        {
            Logger.LogError("Unexpected error while getting paging metadata to re-paginate table");
        }
    }

    protected abstract void SetServerValidationErrors(IList<Error> errors);

    protected virtual void CloseEditorDialog() => DialogService.Close();

    protected async ValueTask<bool> DeleteModelFromApi(IDeleteByIdCommand rq, string modelType)
    {
        var result = await Sender.Send(rq, CT);

        return await result.MatchFirstAsync(
            async _ =>
            {
                await HandlePageStateAfterDelete();

                return true;
            },
            error =>
            {
                HandleApiDeleteErrors(rq.Id, error, modelType);

                return Task.FromResult(false);
            }
        );
    }

    private async ValueTask HandlePageStateAfterDelete()
    {
        var visiblePageAfterDelete = ViewModels.Count == 1
            ? CurrentPage - 1
            : CurrentPage;

        await AfterSuccessfulChange(visiblePageAfterDelete);
    }

    protected abstract Task AfterSuccessfulChange(int apiPage);

    private void HandleApiDeleteErrors(int id, Error error, string modelType)
    {
        if (error.Type is ErrorType.NotFound)
        {
            Logger.LogWarning("{ModelType} '{Id}' do not exist anymore in database", modelType, id);
        }
        else
        {
            Logger.LogError("Unexpected failure while trying to delete {ModelType} '{Id}'", modelType, id);
        }
    }

    protected async ValueTask<bool> GetUserConfirmation(string name, string modelType) =>
        await DialogService.Confirm(
            $"Are you sure to delete {modelType} with name <em>{name}</em>?",
            "Confirm deletion",
            new()
            {
                OkButtonText = "Confirm",
                CancelButtonText = "Cancel",
                Draggable = true,
                CloseDialogOnOverlayClick = true,
            }
        ) ?? false;

    protected void PagerChangeHandler(PagerEventArgs changeData)
    {
        CurrentPage = ToApiPage(changeData.PageIndex);
        CurrentPageSize = changeData.Top;
    }

    /// <summary>
    ///     Radzen DataGrid uses 0-based page index, but API uses 1-based page index.
    /// </summary>
    protected static int ToGridPage(int apiPage) => apiPage - 1;

    /// <inheritdoc cref="ToGridPage" />
    protected static int ToApiPage(int gridPage) => gridPage + 1;
}
