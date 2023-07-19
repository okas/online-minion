using System.Globalization;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.HttpHeaders;

namespace OnlineMinion.RestApi;

public abstract class ApiControllerBase : ControllerBase
{
    /// <summary>
    ///     Sets response headers of paging related data.
    /// </summary>
    /// <param name="values">Paging meta info to set into response headers.</param>
    protected void SetPagingHeaders(PagingMetaInfo values)
    {
        var headers = HttpContext.Response.Headers;

        headers[CustomHeaderNames.PagingRows] = values.Rows.ToString(NumberFormatInfo.InvariantInfo);
        headers[CustomHeaderNames.PagingSize] = values.Size.ToString(NumberFormatInfo.InvariantInfo);
        headers[CustomHeaderNames.PagingPages] = values.Pages.ToString(NumberFormatInfo.InvariantInfo);
    }

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
                ModelState.AddModelError(
                    key,
                    string.IsNullOrWhiteSpace(v) ? "<Missing error message>" : v
                );
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

    /// <summary>
    ///     Checks id equality between route parameter and request body.
    ///     If they are not equal, adds error to ModelState and returns <see cref="ValidationProblemDetails" /> as
    ///     <see cref="BadRequestResult" />.
    /// </summary>
    /// <param name="id">Route parameter.</param>
    /// <param name="req">Request body as <see cref="IHasIntId" /></param>
    /// <returns></returns>
    protected ActionResult? CheckId(int id, IHasIntId req)
    {
        if (id == req.Id)
        {
            return null;
        }

        ModelState.AddModelError("id", "Id in route and in body are not the same.");

        return ValidationProblem();
    }
}
