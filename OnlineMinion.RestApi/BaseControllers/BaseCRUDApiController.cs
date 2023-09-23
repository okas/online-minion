using System.ComponentModel.DataAnnotations;
using System.Globalization;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.RestApi.Init;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace OnlineMinion.RestApi.BaseControllers;

public abstract class BaseCRUDApiController(ISender sender) : BaseApiController
{
    protected readonly ISender Sender = sender;

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

        var result = await Sender.Send(req, ct);

        return result.MatchFirst(
            pagingMetaInfo =>
            {
                SetPagingHeaders(pagingMetaInfo);
                return NoContent();
            },
            CreateApiProblemResult
        );
    }

    protected abstract IGetPagingInfoRequest PagingMetaInfoRequestFactory(int pageSize);

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
