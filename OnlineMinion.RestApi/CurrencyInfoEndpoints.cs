using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using OnlineMinion.Contracts.CurrencyInfo.Requests;
using OnlineMinion.Contracts.CurrencyInfo.Responses;

namespace OnlineMinion.RestApi;

public static class CurrencyInfoEndpoints
{
    public const string GroupName = "CurrencyInfo";

    public static IEndpointRouteBuilder MapAll(IEndpointRouteBuilder app)
    {
        var v1 = app.NewVersionedApi("CurrencyInfo")
            .HasApiVersion(1)
            .MapGroup("api/currency-info");

        v1.MapGet("", GetCurrencies);

        return app;
    }

    public static async Task<Results<Ok<IAsyncEnumerable<CurrencyInfoResp>>, ProblemHttpResult>> GetCurrencies(
        ISender           sender,
        CancellationToken ct
    )
    {
        var rq = new GetCurrenciesReq();
        var result = await sender.Send(rq, ct);

        return TypedResults.Ok(result.Value);
    }
}
