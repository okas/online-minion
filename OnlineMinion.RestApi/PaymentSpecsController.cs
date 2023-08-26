using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Shared.Responses;
using OnlineMinion.Data;
using OnlineMinion.Data.Entities.Shared;
using OnlineMinion.RestApi.Configuration;
using OnlineMinion.RestApi.Shared.Requests;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace OnlineMinion.RestApi;

[ApiVersion("1")]
public class PaymentSpecsController(ISender sender, ILogger<PaymentSpecsController> logger) : ApiControllerBase(sender)
{
    private readonly ILogger<PaymentSpecsController> _logger = logger;

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
        var result = await Sender.Send(new CheckPaymentSpecUniqueNewReq(name), ct);

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
        var result = await Sender.Send(new CheckPaymentSpecUniqueExistingReq(name, exceptId), ct);

        return result.MatchFirst(
            _ => NoContent(),
            firstError => firstError.Type == ErrorType.Conflict
                ? Conflict()
                : CreateApiProblemResult(firstError)
        );
    }

    [HttpGet("{id}")]
    [ProducesResponseType<PaymentSpecResp>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] GetPaymentSpecByIdReq rq,
        CancellationToken                 ct
    ) => await Sender.Send(rq, ct) is { } model ? Ok(model) : NotFound();

    [HttpGet]
    [EnableCors(ApiCorsOptionsConfigurator.ExposedHeadersPagingMetaInfo)]
    [ProducesResponseType(typeof(IAsyncEnumerable<PaymentSpecResp>), StatusCodes.Status200OK)]
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
    public async Task<IActionResult> GetSomePaged(
        [FromQuery] BaseGetSomeModelsPagedReq<PaymentSpecResp> rq,
        CancellationToken                                      ct
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await Sender.Send(rq, ct);

        return result.MatchFirst(
            envelope =>
            {
                SetPagingHeaders(envelope.Paging);
                return Ok(envelope.Result);
            },
            firstError =>
            {
                logger.LogError("Error while getting paged models: {Error}", firstError);
                return CreateApiProblemResult(firstError);
            }
        );
    }

    [HttpPost]
    [ProducesResponseType<ModelIdResp>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(CreatePaymentSpecReq rq, CancellationToken ct)
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
    public async Task<IActionResult> Update(int id, UpdatePaymentSpecReq rq, CancellationToken ct)
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
        [FromRoute] DeletePaymentSpecReq rq,
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
        new GetPagingMetaInfoReq<BasePaymentSpec>(pageSize);
}
