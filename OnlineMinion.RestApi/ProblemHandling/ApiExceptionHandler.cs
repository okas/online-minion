using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OnlineMinion.Application.Exceptions;

namespace OnlineMinion.RestApi.ProblemHandling;

/// <summary>
///     Reacts to known API errors as of now only tweaks <see cref="HttpResponse" /> status and logs exception.
///     Will not return "handled" flag.
/// </summary>
/// <param name="logger"></param>
/// <remarks>
///     API is supposed to be customize instances of <see cref="Microsoft.AspNetCore.Mvc.ProblemDetails" />,
///     this is done by <see cref="ApiExceptionProblemDetailsMapper" /> class and this in turn is used by
///     <see cref="ProblemDetailsOptions.CustomizeProblemDetails">ProblemDetailsOptions.CustomizeProblemDetails</see>.
///     <br />
///     See examples about DI configuration to wire thing up.
/// </remarks>
/// <example>
///     <code>
///     #region API Problem handling setup
///     services.AddSingleton&lt;IExceptionProblemDetailsMapper, ApiExceptionProblemDetailsMapper&gt;();
///     services.ConfigureOptions&lt;ProblemDetailsOptionsConfigurator&gt;();
///     services.AddProblemDetails();
///     services.AddExceptionHandler&lt;ApiExceptionHandler&gt;();
///     #endregion
///     </code>
/// </example>
public class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        switch (exception)
        {
            case ConflictException ex:
            {
                logger.LogWarning(
                    ex,
                    "Conflict exception occurred during saving to database, request: `{Method} {Request}`",
                    httpContext.Request.Method,
                    httpContext.Request.Path
                );
                httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                break;
            }
        }

        // It is intentionally only setting to log and tweak response if needed.
        return new(false);
    }
}
