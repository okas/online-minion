using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineMinion.Contracts.CurrencyInfo.Requests;
using OnlineMinion.Contracts.CurrencyInfo.Responses;
using OnlineMinion.RestApi.BaseControllers;

namespace OnlineMinion.RestApi;

[ApiVersion("1")]
public class CurrencyInfoController(ISender sender) : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(IAsyncEnumerable<CurrencyInfoResp>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrencies(CancellationToken ct)
    {
        var rq = new GetCurrenciesReq();
        var result = await sender.Send(rq, ct);

        return result.MatchFirst(Ok, CreateApiProblemResult);
    }
}
