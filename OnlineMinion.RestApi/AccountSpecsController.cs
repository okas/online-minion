using System.ComponentModel.DataAnnotations;
using ErrorOr;
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

[ApiVersion("1")]
public class AccountSpecsController : ApiControllerBase
{
    public AccountSpecsController(ISender sender) : base(sender) { }

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
        var pagingMetaInfo = await Sender.Send(new GetPagingMetaInfoReq<AccountSpec>(pageSize), ct);

        SetPagingHeaders(pagingMetaInfo);

        return NoContent();
    }

    /// <summary>
    ///     Unique name validation for new create workflow.
    /// </summary>
    /// <remarks>Check, if any resource already uses interested name.</remarks>
    /// <param name="name">Interested new <b>name</b>.</param>
    /// <param name="ct"></param>
    [HttpHead("validate-available-name/{name:required:length(2,50)}")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CheckUniqueNew(string name, CancellationToken ct) =>
        await Sender.Send(new CheckAccountSpecUniqueNewReq(name), ct) ? NoContent() : Conflict();

    /// <summary>
    ///     Uniqueness validation for update workflow.
    /// </summary>
    /// <remarks>Check, if any <b>other existing</b> resource already uses name.</remarks>
    /// <param name="name">Name to validate.</param>
    /// <param name="exceptId">Id of resource that is being updated, must be excluded from check. </param>
    /// <param name="ct"></param>
    [HttpHead("validate-available-name/{name:required:length(2,50)}/except-id/{exceptId:min(1)}")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CheckUniqueExisting(string name, int exceptId, CancellationToken ct) =>
        await Sender.Send(new CheckAccountSpecUniqueExistingReq(name, exceptId), ct) ? NoContent() : Conflict();

    [HttpGet("{id}")]
    [ProducesResponseType<AccountSpecResp>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] GetAccountSpecByIdReq cmd,
        CancellationToken                 ct
    ) => await Sender.Send(cmd, ct) is { } model ? Ok(model) : NotFound();

    [HttpGet]
    [EnableCors(ApiCorsOptionsConfigurator.ExposedHeadersPagingMetaInfo)]
    [ProducesResponseType(typeof(IAsyncEnumerable<AccountSpecResp>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status404NotFound)]
    [SwaggerResponseHeader(
        StatusCodes.Status200OK,
        CustomHeaderNames.PagingRows,
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
    public async Task<IActionResult> GetSome([FromQuery] GetAccountSpecsReq rq, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await Sender.Send(rq, ct);

        SetPagingHeaders(result.Paging);

        return Ok(result.Result);
    }

    [HttpPost]
    [ProducesResponseType<ModelIdResp>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(CreateAccountSpecReq req, CancellationToken ct)
    {
        var result = await Sender.Send(req, ct);

        return result.MatchFirst(
            idResp => CreatedAtAction(nameof(GetById), new { idResp.Id, }, idResp),
            error => CreateApiProblemResult(error)
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

        var result = await Sender.Send(req, ct);

        return result.MatchFirst(
            _ => NoContent(),
            error => CreateApiProblemResult(error, GetInstanceUrl(nameof(GetById), req.Id))
        );
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] DeleteAccountSpecReq req,
        CancellationToken                ct
    )
    {
        var result = await Sender.Send(req, ct);

        return result.MatchFirst(
            _ => NoContent(),
            error => error.Type switch
            {
                ErrorType.NotFound => NotFound(),
                _ => CreateApiProblemResult(error, GetInstanceUrl(nameof(GetById), req.Id)),
            }
        );
    }
}
