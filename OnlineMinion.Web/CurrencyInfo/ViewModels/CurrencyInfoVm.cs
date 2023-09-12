using OnlineMinion.Contracts.CurrencyInfo.Responses;

namespace OnlineMinion.Web.CurrencyInfo.ViewModels;

public readonly record struct CurrencyInfoVm
{
    private readonly CurrencyInfoResp _response;

    public CurrencyInfoVm(CurrencyInfoResp response) => _response = response;

    public string IsoCode => _response.IsoCode;

    public string Symbol => _response.Symbol;

    public string Display => string.Equals(IsoCode, Symbol, StringComparison.OrdinalIgnoreCase)
        ? IsoCode
        : $"{IsoCode} ({Symbol})";
}
