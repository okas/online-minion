using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineMinion.Contracts.CurrencyInfo.Requests;
using OnlineMinion.RestApi.BaseControllers;

namespace OnlineMinion.RestApi;

[ApiVersion("1")]
public class CurrencyInfoController(ISender sender) : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetCurrencies(CancellationToken ct)
    {
        var rq = new GetCurrenciesReq();
        var result = await sender.Send(rq, ct);

        return result.MatchFirst(
            Ok,
            firstError => CreateApiProblemResult(firstError)
        );
    }
}
