using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Contracts.HttpHeaders;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Data.Entities;
using OnlineMinion.RestApi.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace OnlineMinion.RestApi;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
[ApiController]
public class AccountSpecsController : Controller
{
    private readonly IMediator _mediator;

    public AccountSpecsController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    ///     To probe some paging related data about this resource.
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="ct"></param>
    [HttpHead]
    [EnableCors(ApiCorsOptionsConfigurator.ExposedHeadersPagingMetaInfo)]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Using page size, get count of total items and count of pages.")]
    [SwaggerResponseHeader(
        StatusCodes.Status204NoContent,
        CustomHeaderNames.PagingTotalItems,
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
        [FromQuery][Range(1, 50)][DefaultValue(10)] int pageSize,
        CancellationToken                               ct
    )
    {
        var pagingMetaInfo = await _mediator.Send(new GetPagingMetaInfoReq<AccountSpec>(pageSize), ct);

        SetPagingHeaders(pagingMetaInfo);

        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountSpecResp?>> GetById(
        [FromRoute] GetAccountSpecByIdReq cmd,
        CancellationToken                 ct
    ) => await _mediator.Send(cmd, ct);

    [HttpGet]
    [EnableCors(ApiCorsOptionsConfigurator.ExposedHeadersPagingMetaInfo)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status404NotFound)]
    [SwaggerResponseHeader(
        StatusCodes.Status200OK,
        CustomHeaderNames.PagingTotalItems,
        "integer",
        "Total items of resource."
    )]
    [SwaggerResponseHeader(
        StatusCodes.Status200OK,
        CustomHeaderNames.PagingSize,
        "integer",
        "Page size used."
    )]
    [SwaggerResponseHeader(
        StatusCodes.Status200OK,
        CustomHeaderNames.PagingPages,
        "integer",
        "Pages, based on provided page size."
    )]
    public async Task<IAsyncEnumerable<AccountSpecResp>> Get(
        [FromQuery] GetAccountSpecsReq req,
        CancellationToken              ct
    )
    {
        var result = await _mediator.Send(req, ct);

        SetPagingHeaders(result.Paging);

        return result.Result;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAccountSpecReq req, CancellationToken ct)
    {
        var resp = await _mediator.Send(req, ct);
        // TODO: Add check, if result is unfavorable.

        return CreatedAtAction(nameof(GetById), new { resp.Id, }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(UpdateAccountSpecReq req, CancellationToken ct) =>
        await RunIdempotentAction(req, ct);

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] DeleteAccountSpecReq req, CancellationToken ct) =>
        await RunIdempotentAction(req, ct);

    private void SetPagingHeaders(PagingMetaInfo pagingMetaInfo)
    {
        Response.Headers[CustomHeaderNames.PagingTotalItems] = pagingMetaInfo.TotalItems.ToString();
        Response.Headers[CustomHeaderNames.PagingSize] = pagingMetaInfo.Size.ToString();
        Response.Headers[CustomHeaderNames.PagingPages] = pagingMetaInfo.Pages.ToString();
    }
}
