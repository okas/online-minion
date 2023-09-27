using Microsoft.AspNetCore.Http;
using OnlineMinion.Data.Exceptions;

namespace OnlineMinion.RestApi.ProblemHandling;

public sealed class ApiExceptionProblemDetailsMapper : IExceptionProblemDetailsMapper
{
    public void MapExceptions(ProblemDetailsContext context)
    {
        switch (context.Exception)
        {
            case ConflictException conflictException:
            {
                context.ProblemDetails = new HttpValidationProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = conflictException.Message,
                    Errors = CreateErrorsExtension(conflictException.Errors),
                };
                break;
            }

            case { } ex:
            {
                context.ProblemDetails.Title = ex.Message;
                context.ProblemDetails.Status = StatusCodes.Status500InternalServerError;
                break;
            }
        }
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
