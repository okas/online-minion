using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Contracts.HttpHeaders;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Data.Entities;
using OnlineMinion.RestApi.AppMessaging.Requests;
using OnlineMinion.RestApi.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace OnlineMinion.RestApi;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
[ApiController]
public class AccountSpecsController : ApiControllerBase
{
    private readonly ISender _sender;
    public AccountSpecsController(ISender sender) => _sender = sender;

    /// <summary>
    ///     To probe some paging related data about this resource.
    /// </summary>
    /// <param name="ct"></param>
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
    public async Task<IActionResult> PagingMetaInfo(
        CancellationToken             ct,
        [FromQuery][Range(1, 50)] int pageSize = 10
    )
    {
        var pagingMetaInfo = await _sender.Send(new GetPagingMetaInfoReq<AccountSpec>(pageSize), ct);

        SetPagingHeaders(pagingMetaInfo);

        return NoContent();
    }

    [HttpGet("{id}")]
    [ProducesResponseType<AccountSpecResp>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] GetAccountSpecByIdReq cmd,
        CancellationToken                 ct
    ) => await _sender.Send(cmd, ct) is { } model ? Ok(model) : NotFound();

    [HttpGet]
    [EnableCors(ApiCorsOptionsConfigurator.ExposedHeadersPagingMetaInfo)]
    [ProducesResponseType(typeof(IAsyncEnumerable<AccountSpecResp>), StatusCodes.Status200OK)]
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
    public async Task<IActionResult> GetSome(
        [FromQuery] GetAccountSpecsReq req,
        CancellationToken              ct
    )
    {
        var result = await _sender.Send(req, ct);

        SetPagingHeaders(result.Paging);

        return Ok(result.Result);
    }

    [HttpPost]
    [ProducesResponseType<ModelIdResp>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(CreateAccountSpecReq req, CancellationToken ct)
    {
        var result = await _sender.Send(req, ct);

        return result.MatchFirst<IActionResult>(
            idResp => CreatedAtAction(nameof(GetById), new { idResp.Id, }, idResp),
            error => CreateProblemResult(error)
        );
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, UpdateAccountSpecReq req, CancellationToken ct)
    {
        if (CheckId(id, req) is { } actionResult)
        {
            return actionResult;
        }

        var result = await _sender.Send(req, ct);

        return result.MatchFirst<IActionResult>(
            _ => NoContent(),
            error => CreateProblemResult(error, GetInstanceUrl(nameof(GetById), req.Id))
        );
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] DeleteAccountSpecReq req,
        CancellationToken                ct
    ) => await _sender.Send(req, ct) ? NoContent() : NotFound();
}
