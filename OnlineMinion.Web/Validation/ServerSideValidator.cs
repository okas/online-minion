using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace OnlineMinion.Web.Validation;

public class ServerSideValidator : ComponentBase
{
    private ValidationMessageStore _messageStore = null!;

    [CascadingParameter]
    public EditContext CurrentEditContext { get; set; } = null!;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (CurrentEditContext == null)
        {
            throw new InvalidOperationException(
                $"{nameof(ServerSideValidator)} requires a cascading " +
                $"parameter of type {nameof(EditContext)}. For example, you can use {nameof(ServerSideValidator)} " +
                $"inside an {nameof(EditForm)}."
            );
        }

        _messageStore = new(CurrentEditContext);

        CurrentEditContext.OnValidationRequested += (s, e) => _messageStore.Clear();

        CurrentEditContext.OnFieldChanged += (s, e) => _messageStore.Clear(e.FieldIdentifier);
    }

    public void DisplayErrors(IDictionary<string, object> errors)
    {
        foreach (var fieldErrors in errors.Where(pair => pair.Value is IEnumerable<object>))
        {
            var fieldName = GetFieldName(fieldErrors.Key);

            foreach (var message in (IEnumerable<object>)fieldErrors.Value)
            {
                _messageStore.Add(CurrentEditContext.Field(fieldName), GetErrorMessage(message));
            }
        }

        CurrentEditContext.NotifyValidationStateChanged();
    }

    public static string GetFieldName(string rawKey) => rawKey.Split('.')[^1];

    private static string GetErrorMessage(object rawMessage) => rawMessage.ToString() ?? "#error";
}
