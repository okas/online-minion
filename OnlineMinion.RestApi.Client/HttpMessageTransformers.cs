using System.Net.Http.Json;
using ErrorOr;
using Microsoft.AspNetCore.Http;

namespace OnlineMinion.RestApi.Client;

public static class HttpMessageTransformers
{
    /// <exception cref="InvalidOperationException">
    ///     If message content is not understood,
    ///     it is not <see cref="HttpValidationProblemDetails" />.
    /// </exception>
    public static async Task<ErrorOr<TValue>> TransformConflictHttpResponse<TValue>(
        HttpResponseMessage message,
        CancellationToken   ct
    )
    {
        var validationProblemDetails = await ReadHttpValidationProblemDetails(message, ct).ConfigureAwait(false);

        // TODO: Try to get data from ProblemDetails object too?

        return Error.Conflict(
            validationProblemDetails.Title,
            validationProblemDetails.Detail,
            validationProblemDetails.Errors.CastToObjectValues()
        );
    }

    /// <exception cref="InvalidOperationException">
    ///     If message content is not understood,
    ///     it is not <see cref="HttpValidationProblemDetails" />.
    /// </exception>
    public static async Task<ErrorOr<TValue>> TransformBadRequestHttpResponse<TValue>(
        HttpResponseMessage message,
        CancellationToken   ct
    )
    {
        var validationProblemDetails = await ReadHttpValidationProblemDetails(message, ct).ConfigureAwait(false);

        // TODO: Try to get data from ProblemDetails object too?

        return Error.Validation(
            validationProblemDetails.Title,
            validationProblemDetails.Detail,
            validationProblemDetails.Errors.CastToObjectValues()
        );
    }

    private static async Task<HttpValidationProblemDetails> ReadHttpValidationProblemDetails(
        HttpResponseMessage message,
        CancellationToken   ct
    ) => await message.Content.ReadFromJsonAsync<HttpValidationProblemDetails>(ct).ConfigureAwait(false)
         ?? throw ExceptionHelpers.CreateForUnknownResponse(nameof(HttpValidationProblemDetails));

    private static Dictionary<string, object> CastToObjectValues(this IDictionary<string, string[]> input) =>
        input.ToDictionary(
            e => e.Key,
            e => e.Value as object,
            StringComparer.OrdinalIgnoreCase
        );
}
