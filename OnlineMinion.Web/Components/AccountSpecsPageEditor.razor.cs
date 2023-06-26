using ErrorOr;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OnlineMinion.Contracts.AppMessaging;
using OnlineMinion.Web.Components.Forms;

namespace OnlineMinion.Web.Components;

public partial class AccountSpecsPageEditor : ComponentBase
{
    private FluentValidator _fluentValidatorRef = null!;
    private bool _isEditorActionDisabledForced;
    private ServerSideValidator _serverSideValidator = null!;

    [Parameter]
    [EditorRequired]
    public BaseUpsertAccountSpecReqData Model { get; set; } = null!;

    [Parameter]
    [EditorRequired]
    public bool IsSubmitting { get; set; }

    [Parameter]
    [EditorRequired]
    public EventCallback<EditContext> OnSubmit { get; set; }

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
