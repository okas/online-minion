using ErrorOr;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Web.Shared.Forms;

namespace OnlineMinion.Web.Components;

public partial class AccountSpecsEditor : ComponentBase, IDisposable
{
    private EditContext? _editContext;
    private FluentValidator _fluentValidatorRef = null!;
    private bool _isEditorActionDisabledForced;
    private string? _modalTitle;
    private ServerSideValidator _serverSideValidator = null!;
    private bool _shouldBeDisabledByFormState = true;

    private bool IsActionDisabled => _shouldBeDisabledByFormState || _isEditorActionDisabledForced || SC.IsBusy;

    [Inject]
    public StateContainer SC { get; set; } = default!;

    [Parameter]
    [EditorRequired]
    public BaseUpsertAccountSpecReqData? Model { get; set; }

    [Parameter]
    [EditorRequired]
    public EventCallback<EditContext> OnSubmit { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    void IDisposable.Dispose() => SC.OnChange -= StateHasChanged;


    protected override void OnParametersSet()
    {
        if (Model is not null)
        {
            _editContext = new(Model);
            _editContext.OnValidationStateChanged += OnValidationStateChangedHandler;
        }
        else if (_editContext is not null)
        {
            _editContext.OnValidationStateChanged -= OnValidationStateChangedHandler;
            _editContext = null;
        }
    }

    protected override void OnInitialized() => SC.OnChange += StateHasChanged;

    public void ResetEditor()
    {
        _editContext!.MarkAsUnmodified();
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
