using Microsoft.AspNetCore.Http;
using OnlineMinion.Application.Exceptions;

namespace OnlineMinion.RestApi.ProblemHandling;

/// <summary>
///     Maps known exceptions ins API to <see cref="Microsoft.AspNetCore.Mvc.ProblemDetails" />, customizing them as
///     necessary.
/// </summary>
/// <remarks>
///     This class is supposed to be updated, for expected <see cref="Microsoft.AspNetCore.Mvc.ProblemDetails" /> to be
///     returned
///     from API.
/// </remarks>
public sealed class ApiExceptionProblemDetailsMapper : IExceptionProblemDetailsMapper
{
    public void MapExceptions(ProblemDetailsContext context)
    {
        var exception = context.Exception;
        switch (exception)
        {
            case ConflictException conflictEx:
            {
                context.ProblemDetails = new HttpValidationProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Errors = CreateErrorsExtension(conflictEx.Errors),
                };
                break;
            }

            case { } ex:
            {
                if (context.HttpContext.Response.StatusCode < 500)
                {
                    context.ProblemDetails.Status = context.HttpContext.Response.StatusCode;
                }

                break;
            }
        }

        context.ProblemDetails.Detail = exception?.InnerException?.Message ?? exception?.Message;
    }

    private static Dictionary<string, string[]> CreateErrorsExtension(
        IEnumerable<ConflictException.ErrorDescriptor> input
    )
    {
        var result = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        // Merge all directories from all errors into one dictionary.
        foreach (var errorDetail in input.SelectMany(e => e.Details))
        {
            result[errorDetail.Key] = result.TryGetValue(errorDetail.Key, out var errors)
                ? errors.Concat(errorDetail.Value).ToArray()
                : errorDetail.Value;
        }

        return result;
    }
}
