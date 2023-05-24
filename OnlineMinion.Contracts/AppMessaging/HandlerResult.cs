using OnlineMinion.Contracts.Responses;

namespace OnlineMinion.Contracts.AppMessaging;

public record HandlerResult<TData>(
    TData                          Data,
    bool                           IsSuccess,
    string?                        ErrorMessage = null,
    IDictionary<string, string[]>? Errors       = null,
    IDictionary<string, object?>?  Extensions   = null
)
{
    public static HandlerResult<TData> Success(TData data) => new(data, true);

    public static HandlerResult<TData> Failure(string errorMessage, IDictionary<string, string[]>? errors = null) =>
        new(default!, false, errorMessage, errors);

    public static HandlerResult<ModelIdResp?> Failure(
        string                       errorMessage,
        IDictionary<string, object?> errorExtensions
    ) => new(default!, false, errorMessage, Extensions: errorExtensions);
}
