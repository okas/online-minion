using System.Globalization;
using ErrorOr;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Application.Contracts.CurrencyInfo.Requests;
using OnlineMinion.Application.Contracts.CurrencyInfo.Responses;

namespace OnlineMinion.Application.CurrencyInfo.Handlers;

[UsedImplicitly]
internal sealed class GetCurrenciesReqHlr
    : IRequestHandler<GetCurrenciesReq, ErrorOr<IAsyncEnumerable<CurrencyInfoResp>>>
{
    public Task<ErrorOr<IAsyncEnumerable<CurrencyInfoResp>>> Handle(GetCurrenciesReq rq, CancellationToken ct) =>
        Task.FromResult(ErrorOrFactory.From(GetCurrencyData()));

    private static IAsyncEnumerable<CurrencyInfoResp> GetCurrencyData() =>
        CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            .Select(culture => new RegionInfo(culture.Name))
            .DistinctBy(r => r.ISOCurrencySymbol)
            .Select(region => new CurrencyInfoResp(region.ISOCurrencySymbol, region.CurrencySymbol))
            .OrderBy(c => c.IsoCode, StringComparer.OrdinalIgnoreCase)
            .ToAsyncEnumerable();
}
