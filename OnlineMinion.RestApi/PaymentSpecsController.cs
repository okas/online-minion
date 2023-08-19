using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineMinion.Contracts.AppMessaging;
using OnlineMinion.Contracts.HttpHeaders;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Data.BaseEntities;
using OnlineMinion.RestApi.AppMessaging.Requests;
using OnlineMinion.RestApi.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace OnlineMinion.RestApi;

[ApiVersion("1")]
public class PaymentSpecsController : ApiControllerBase
{
    public PaymentSpecsController(ISender sender) : base(sender) { }

    [HttpGet("{id}")]
    [ProducesResponseType<PaymentSpecResp>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] GetPaymentSpecByIdReq cmd,
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
    public async Task<IActionResult> GetSome([FromQuery] BaseGetSomeReq<PaymentSpecResp> rq, CancellationToken ct)
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
    public async Task<IActionResult> Create(CreatePaymentSpecReq req, CancellationToken ct)
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
    public async Task<IActionResult> Update(int id, UpdatePaymentSpecReq req, CancellationToken ct)
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
        [FromRoute] DeletePaymentSpecReq req,
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

    protected override IPagedResourceRequest<BaseEntity> PagingMetaInfoRequestFactory(int pageSize) =>
        new GetPagingMetaInfoReq<BasePaymentSpec>(pageSize);
}
