using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using OnlineMinion.Application.Contracts.CurrencyInfo.Requests;
using OnlineMinion.Application.Contracts.CurrencyInfo.Responses;

namespace OnlineMinion.RestApi;

public static class CurrencyInfoEndpoints
{
    public const string GroupName = "Currency Info";

    public static IEndpointRouteBuilder MapAll(IEndpointRouteBuilder app)
    {
        var apiV1 = app.NewVersionedApi("CurrencyInfo")
            .MapGroup("api/currency-info")
            .HasApiVersion(1)
            .MapToApiVersion(1);

        apiV1.MapGet("", GetCurrencies);

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
