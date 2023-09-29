using System.Net.Http.Json;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using OnlineMinion.Application.Contracts;

namespace OnlineMinion.RestApi.Client.Helpers;

public static class HttpMessageTransformers
{
    /// <exception cref="InvalidOperationException">
    ///     If message content is not understood,
    ///     it is not <see cref="HttpValidationProblemDetails" />.
    /// </exception>
    public static async Task<Error> TransformConflictHttpResponse(
        this HttpResponseMessage message,
        CancellationToken        ct
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
    public static async Task<Error> TransformBadRequestHttpResponse(
        this HttpResponseMessage message,
        CancellationToken        ct
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

    /// <summary>
    ///     Reads Paging info from response headers.
    /// </summary>
    public static ErrorOr<PagingMetaInfo> GetPagingMetaInfo(this HttpResponseMessage response, int? page = default)
    {
        var size = response.Headers.GetHeaderFirstValue<int?>(CustomHeaderNames.PagingSize);
        var totalItems = response.Headers.GetHeaderFirstValue<int?>(CustomHeaderNames.PagingRows);

        if (size is null || totalItems is null)
        {
            return Error.Failure(description: "Paging info not found in header.");
        }

        return page.HasValue
            ? new(totalItems.Value, size.Value, page.Value)
            : new PagingMetaInfo(totalItems.Value, size.Value);
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
