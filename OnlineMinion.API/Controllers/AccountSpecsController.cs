using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OnlineMinion.API.Configurators;
using OnlineMinion.Contracts.Commands;
using OnlineMinion.Contracts.HttpHeaders;
using OnlineMinion.Contracts.Queries;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Data.Entities;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace OnlineMinion.API.Controllers;

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
        var pagingMetaInfo = await _mediator.Send(new GetPagingMetaInfoQry<AccountSpec>(pageSize), ct);

        Response.Headers[CustomHeaderNames.PagingTotalItems] = pagingMetaInfo.TotalItems.ToString();
        Response.Headers[CustomHeaderNames.PagingSize] = pagingMetaInfo.Size.ToString();
        Response.Headers[CustomHeaderNames.PagingPages] = pagingMetaInfo.Pages.ToString();

        return NoContent();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AccountSpecResp?>> GetById(int id, CancellationToken ct) =>
        await _mediator.Send(new GetAccountSpecByIdQry(id), ct);

    [HttpGet]
    public async Task<ActionResult<BasePagedResult<AccountSpecResp>>> Get(
        [FromQuery] GetAccountSpecsQry qry,
        CancellationToken              ct
    ) => await _mediator.Send(qry, ct);

    [HttpPost]
    public async Task<IActionResult> Create(CreateAccountSpecCmd cmd, CancellationToken ct)
    {
        var resp = await _mediator.Send(cmd, ct);

        return CreatedAtAction(nameof(GetById), new { resp.Id }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] UpdateAccountSpecCmd cmd, CancellationToken ct) =>
        await RunIdempotentAction(cmd, ct);

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] DeleteAccountSpecCmd cmd, CancellationToken ct) =>
        await RunIdempotentAction(cmd, ct);

    private async Task<IActionResult> RunIdempotentAction(IRequest<int> cmd, CancellationToken ct)
    {
        var count = await _mediator.Send(cmd, ct);

        return count == 0 ? NotFound() : NoContent();
    }
}
