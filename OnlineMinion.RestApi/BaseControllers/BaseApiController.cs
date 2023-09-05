using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineMinion.RestApi.BaseControllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public abstract class BaseApiController : ControllerBase
{
    /// <summary>
    ///     Sets ModelState and "converts" <see cref="Error" /> to <see cref="IActionResult" />.
    /// </summary>
    /// <param name="error">Error to convert to action result.</param>
    /// <param name="instanceUrl">Optional URL similar to <code>GET /api/[controller]/{id}</code>.</param>
    protected ActionResult CreateApiProblemResult(Error error, string? instanceUrl = null)
    {
        if (error.Dictionary is not null)
        {
            SaveToModelState(error.Dictionary);
        }

        return error.Type switch
        {
            ErrorType.Conflict => ValidationProblem(error.Description, statusCode: StatusCodes.Status409Conflict),
            ErrorType.Validation => ValidationProblem(error.Description, instanceUrl),
            _ => Problem(error.Description, title: error.Code, instance: instanceUrl),
        };
    }

    private void SaveToModelState(IDictionary<string, object> errorMetadata)
    {
        foreach (var (key, data) in errorMetadata)
        {
            var values = data as IEnumerable<string>
                         ?? new[] { data.ToString() ?? string.Empty, };

            foreach (var v in values)
            {
                var errorMessage = string.IsNullOrWhiteSpace(v) ? "<Missing error message>" : v;
                ModelState.AddModelError(key, errorMessage);
            }
        }
    }

    /// <inheritdoc cref="UrlHelperExtensions.Action(Microsoft.AspNetCore.Mvc.IUrlHelper)" />
    /// <param name="getByIdActionName">
    ///     Name of action, one that produces similar to <code>GET /api/[controller]/{id}</code>.
    /// </param>
    /// <param name="id">Id value that came from route parameter.</param>
    protected virtual string? GetInstanceUrl(string getByIdActionName, int id) =>
        Url.Action(getByIdActionName, new { id, });
}
