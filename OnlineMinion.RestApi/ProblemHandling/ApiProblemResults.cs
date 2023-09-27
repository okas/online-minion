using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace OnlineMinion.RestApi.ProblemHandling;

public static class ApiProblemResults
{
    /// <summary>
    ///     Converts <see cref="Error" /> to <see cref="ProblemDetails" /> and returns is as <see cref="ProblemHttpResult" />.
    ///     <br />
    ///     Supported status codes or scenarios: <see cref="StatusCodes.Status400BadRequest" />,
    ///     <see cref="StatusCodes.Status404NotFound" />,
    ///     <see cref="StatusCodes.Status409Conflict" />, all the rest <see cref="StatusCodes.Status500InternalServerError" />.
    /// </summary>
    /// <remarks>
    ///     <see cref="ErrorType" /> is used to determine the the details for the <see cref="ProblemDetails" />, like
    ///     <see cref="ProblemDetails.Status" />, <see cref="ProblemDetails.Detail" /> and so on.
    /// </remarks>
    /// <param name="error">Error to convert to action result.</param>
    public static ProblemHttpResult CreateApiProblemResult(Error error)
        => CreateApiProblemResult(error, null);

    /// <inheritdoc cref="CreateApiProblemResult(Error)" />
    /// <param name="instanceUrl">Optional URL similar to: <code>GET /api/{resource}/{id}</code></param>
    public static ProblemHttpResult CreateApiProblemResult(Error error, string? instanceUrl)
    {
        var problemDetails = error.Type switch
        {
            ErrorType.NotFound => new()
            {
                Status = StatusCodes.Status404NotFound,
                Instance = instanceUrl,
            },

            ErrorType.Conflict => new HttpValidationProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Instance = instanceUrl,
                Errors = ConvertErrors(error.Dictionary),
            },

            ErrorType.Validation => new HttpValidationProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Detail = error.Description,
                Instance = instanceUrl,
                Errors = ConvertErrors(error.Dictionary),
            },

            _ => new ProblemDetails
            {
                Title = error.Code,
                Detail = error.Description,
                Instance = instanceUrl,
                Extensions = error.Dictionary ?? new(StringComparer.OrdinalIgnoreCase),
            },
        };

        return TypedResults.Problem(problemDetails);
    }

    private static Dictionary<string, string[]> ConvertErrors(Dictionary<string, object?>? errorMetadata) =>
        errorMetadata is null
            ? new(StringComparer.OrdinalIgnoreCase)
            : errorMetadata.ToDictionary(pair => pair.Key, ConvertValueToArray, StringComparer.OrdinalIgnoreCase);

    private static string[] ConvertValueToArray(KeyValuePair<string, object?> pair) =>
    (
        pair.Value as IEnumerable<string>
        ?? new[] { pair.Value?.ToString() ?? string.Empty, }
    ).ToArray();
}
