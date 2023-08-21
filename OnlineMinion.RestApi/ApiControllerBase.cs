using System.ComponentModel.DataAnnotations;
using System.Globalization;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.HttpHeaders;
using OnlineMinion.Data.BaseEntities;
using OnlineMinion.RestApi.Configuration;
using OnlineMinion.RestApi.Requests;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace OnlineMinion.RestApi;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected readonly ISender Sender;
    protected ApiControllerBase(ISender sender) => Sender = sender;

    /// <summary>
    ///     To probe some paging related data about this resource.
    /// </summary>
    /// <param name="ct"></param>
    /// <param name="pageSize"></param>
    [HttpHead]
    [EnableCors(ApiCorsOptionsConfigurator.ExposedHeadersPagingMetaInfo)]
    [SwaggerResponse(
        StatusCodes.Status204NoContent,
        "Using page size, get count of total items and count of pages."
    )]
    [SwaggerResponseHeader(
        StatusCodes.Status204NoContent,
        CustomHeaderNames.PagingRows,
        "integer",
        "Total items of resource."
    )]
    [SwaggerResponseHeader(
        StatusCodes.Status204NoContent,
        CustomHeaderNames.PagingSize,
        "integer",
        "Page size used."
    )]
    [SwaggerResponseHeader(
        StatusCodes.Status204NoContent,
        CustomHeaderNames.PagingPages,
        "integer",
        "Pages, based on provided page size."
    )]
    public async Task<IActionResult> PagingMetaInfo(
        [FromQuery][Range(1, 100)] int pageSize = 10,
        CancellationToken              ct       = default
    )
    {
        var req = PagingMetaInfoRequestFactory(pageSize);

        var pagingMetaInfo = await Sender.Send(req, ct);

        SetPagingHeaders(pagingMetaInfo);

        return NoContent();
    }

    protected abstract IPagedResourceRequest<BaseEntity> PagingMetaInfoRequestFactory(int pageSize);

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
