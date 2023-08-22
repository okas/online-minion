using ErrorOr;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OnlineMinion.Web.Shared.Forms;

namespace OnlineMinion.Web.Components;

[UsedImplicitly]
public partial class UpsertEditorWrapper<TModel> : ComponentBase, IDisposable
{
    private EditContext? _editContext;
    private FluentValidator _fluentValidatorRef = null!;
    private bool _isEditorActionDisabledForced;
    private ServerSideValidator _serverSideValidator = null!;
    private bool _shouldBeDisabledByFormState = true;

    protected bool IsActionDisabled => _shouldBeDisabledByFormState || _isEditorActionDisabledForced || SC.IsBusy;

    [Inject]
    public StateContainer SC { get; set; } = default!;

    // TODO: To Cascading Parameter
    [Parameter]
    [EditorRequired]
    public TModel? Model { get; set; }

    [Parameter]
    [EditorRequired]
    public EventCallback<EditContext> OnSubmit { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    [Parameter]
    [EditorRequired]
    public required RenderFragment EditorFormFields { get; set; }

    void IDisposable.Dispose()
    {
        SC.OnChange -= StateHasChanged;
        DetachValidationStateChangedListener();
    }

    protected override void OnParametersSet()
    {
        if (Model is null)
        {
            throw new InvalidOperationException(
                $"Parameter {nameof(Model)} is required for {nameof(UpsertEditorWrapper<TModel>)} component."
            );
        }

        if (_editContext?.Model.Equals(Model) ?? false)
        {
            return;
        }

        DetachValidationStateChangedListener();
        _editContext = new(Model!);
        _editContext.OnValidationStateChanged += OnValidationStateChangedHandler;
    }

    protected override void OnInitialized() => SC.OnChange += StateHasChanged;

    private void DetachValidationStateChangedListener()
    {
        if (_editContext is not null)
        {
            _editContext.OnValidationStateChanged -= OnValidationStateChangedHandler;
        }
    }

    public void ResetEditor()
    {
        _editContext?.MarkAsUnmodified();
        _isEditorActionDisabledForced = false;
    }

    private void OnValidationStateChangedHandler(object? _, ValidationStateChangedEventArgs __)
    {
        _shouldBeDisabledByFormState = _editContext!.GetValidationMessages().Any() || !_editContext.IsModified();
        StateHasChanged();
    }

    public ValueTask<bool> ValidateEditorAsync() => _fluentValidatorRef.ValidateModelAsync();

    public void SetServerValidationErrors(IList<Error> errors)
    {
        HandleValidationAndConflictErrors(errors);
        HandleNotFoundErrors(errors);
    }

    private void HandleValidationAndConflictErrors(IList<Error> errors)
    {
        if (errors.Where(err => err.Type is ErrorType.Validation or ErrorType.Conflict).ToList()
            is not { Count: > 0, } fieldErrors)
        {
            return;
        }

        var flattenedErrors = FlattenFieldErrors(fieldErrors);
        _serverSideValidator.DisplayErrors(flattenedErrors);
    }

    private void HandleNotFoundErrors(IList<Error> errors)
    {
        if (!errors.Any(err => err.Type is ErrorType.NotFound))
        {
            return;
        }

        _serverSideValidator.DisplayErrors("Account specification do not exist on server anymore.");
        _isEditorActionDisabledForced = true;
    }

    private static Dictionary<string, IEnumerable<object>> FlattenFieldErrors(IEnumerable<Error> errors)
    {
        var errorsFromMetadataOrMainError = errors.SelectMany(
            error => error.Dictionary ?? GetMainErrorInfo(error)
        );

        var groupedByFieldNames = errorsFromMetadataOrMainError.GroupBy(
            kvp => kvp.Key,
            StringComparer.OrdinalIgnoreCase
        );

        var flattenFieldErrors = groupedByFieldNames.ToDictionary(
            grouping => grouping.Key,
            grouping => grouping.SelectMany(ToObjectEnumerable),
            StringComparer.OrdinalIgnoreCase
        );

        return flattenFieldErrors;
    }

    private static Dictionary<string, object> GetMainErrorInfo(Error error) =>
        new(StringComparer.OrdinalIgnoreCase) { { error.Code, error.Description }, };

    private static IEnumerable<object> ToObjectEnumerable(KeyValuePair<string, object> kvp) =>
        kvp.Value as IEnumerable<object> ?? new[] { kvp.Value, };
}
