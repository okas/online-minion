using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Contracts.HttpHeaders;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Data.Entities;
using OnlineMinion.RestApi.Configurators;
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
    public async Task<IActionResult> PagingMetaInfo(int pageSize, CancellationToken ct)
    {
        var pagingMetaInfo = await _mediator.Send(new GetPagingMetaInfoReq<AccountSpec>(pageSize), ct);

        Response.Headers[CustomHeaderNames.PagingTotalItems] = pagingMetaInfo.TotalItems.ToString();
        Response.Headers[CustomHeaderNames.PagingSize] = pagingMetaInfo.Size.ToString();
        Response.Headers[CustomHeaderNames.PagingPages] = pagingMetaInfo.Pages.ToString();

        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountSpecResp?>> GetById(
        [FromRoute] GetAccountSpecByIdReq cmd,
        CancellationToken                 ct
    ) => await _mediator.Send(cmd, ct);

    [HttpGet]
    public async Task<ActionResult<BasePagedResult<AccountSpecResp>>> Get(
        [FromQuery] GetAccountSpecsReq req,
        CancellationToken              ct
    ) => await _mediator.Send(req, ct);

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

    private async Task<IActionResult> RunIdempotentAction(IRequest<bool> cmd, CancellationToken ct) =>
        await _mediator.Send(cmd, ct) ? NoContent() : NotFound();
}
