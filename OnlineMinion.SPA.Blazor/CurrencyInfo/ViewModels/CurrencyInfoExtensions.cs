using System.Globalization;

namespace OnlineMinion.SPA.Blazor.CurrencyInfo.ViewModels;

public static class CurrencyInfoExtensions
{
    public static string SymbolizeCurrency(this IEnumerable<CurrencyInfoVm> currencies, string isoCode)
    {
        var currency = currencies.FirstOrDefault(
            x => string.Equals(x.IsoCode, isoCode, StringComparison.OrdinalIgnoreCase)
        );

        return currency != default ? currency.Display : isoCode;
    }

    public static string FormatCurrencyAmount(decimal amount, string isoCode) =>
        $"{isoCode.ToUpper(CultureInfo.CurrentCulture)} {amount.ToString("N2", CultureInfo.CurrentCulture)}";
}
