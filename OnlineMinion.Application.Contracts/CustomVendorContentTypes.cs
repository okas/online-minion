namespace OnlineMinion.Application.Contracts;

public static class CustomVendorContentTypes
{
    private const string Utf8 = "charset=utf-8";

    public const string PaymentSpecCash = "application/vnd.payment-spec.cash";
    public const string PaymentSpecBank = "application/vnd.payment-spec.bank";
    public const string PaymentSpecCrypto = "application/vnd.payment-spec.crypto";
    public const string PaymentSpecCashJson = $"{PaymentSpecCash}+json";
    public const string PaymentSpecBankJson = $"{PaymentSpecBank}+json";
    public const string PaymentSpecCryptoJson = $"{PaymentSpecCrypto}+json";
    public const string PaymentSpecCashJsonCharset = $"{PaymentSpecCashJson}; {Utf8}";
    public const string PaymentSpecBankJsonCharset = $"{PaymentSpecBankJson}; {Utf8}";
    public const string PaymentSpecCryptoJsonCharset = $"{PaymentSpecCryptoJson}; {Utf8}";
}
