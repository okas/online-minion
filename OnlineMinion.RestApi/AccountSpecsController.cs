using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.Contracts.Common.Requests;
using OnlineMinion.Contracts.Common.Responses;
using OnlineMinion.Contracts.HttpHeaders;
using OnlineMinion.Data.BaseEntities;
using OnlineMinion.RestApi.Configuration;
using OnlineMinion.RestApi.Requests;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace OnlineMinion.RestApi;

[ApiVersion("1")]
public class AccountSpecsController : ApiControllerBase
{
    public AccountSpecsController(ISender sender) : base(sender) { }

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
    public async Task<IActionResult> CheckUniqueNew(string name, CancellationToken ct)
    {
        var result = await Sender.Send(new CheckAccountSpecUniqueNewReq(name), ct);

        return result.MatchFirst(
            _ => NoContent(),
            firstError => firstError.Type == ErrorType.Conflict
                ? Conflict()
                : CreateApiProblemResult(firstError)
        );
    }

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
    public async Task<IActionResult> CheckUniqueExisting(string name, int exceptId, CancellationToken ct)
    {
        var result = await Sender.Send(new CheckAccountSpecUniqueExistingReq(name, exceptId), ct);

        return result.MatchFirst(
            _ => NoContent(),
            firstError => firstError.Type == ErrorType.Conflict
                ? Conflict()
                : CreateApiProblemResult(firstError)
        );
    }

    [HttpGet("{id}")]
    [ProducesResponseType<AccountSpecResp>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] GetAccountSpecByIdReq rq,
        CancellationToken                 ct
    ) => await Sender.Send(rq, ct) is { } model ? Ok(model) : NotFound();

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
    public async Task<IActionResult> GetSome([FromQuery] BaseGetSomeReq<AccountSpecResp> rq, CancellationToken ct)
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
    public async Task<IActionResult> Create(CreateAccountSpecReq rq, CancellationToken ct)
    {
        var result = await Sender.Send(rq, ct);

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
    public async Task<IActionResult> Update(int id, UpdateAccountSpecReq rq, CancellationToken ct)
    {
        if (CheckId(id, rq) is { } actionResult)
        {
            return actionResult;
        }

        var result = await Sender.Send(rq, ct);

        return result.MatchFirst(
            _ => NoContent(),
            error => CreateApiProblemResult(error, GetInstanceUrl(nameof(GetById), rq.Id))
        );
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] DeleteAccountSpecReq rq,
        CancellationToken                ct
    )
    {
        var result = await Sender.Send(rq, ct);

        return result.MatchFirst(
            _ => NoContent(),
            error => error.Type switch
            {
                ErrorType.NotFound => NotFound(),
                _ => CreateApiProblemResult(error, GetInstanceUrl(nameof(GetById), rq.Id)),
            }
        );
    }

    protected override IPagedResourceRequest<BaseEntity> PagingMetaInfoRequestFactory(int pageSize) =>
        new GetPagingMetaInfoReq<Data.Entities.AccountSpec>(pageSize);
}
