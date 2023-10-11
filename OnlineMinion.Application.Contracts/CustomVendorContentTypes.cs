namespace OnlineMinion.Application.Contracts;

public static class CustomVendorContentTypes
{
    public const string PaymentSpecCash = "application/vnd.payment-spec.cash";
    public const string PaymentSpecBank = "application/vnd.payment-spec.bank";
    public const string PaymentSpecCrypto = "application/vnd.payment-spec.crypto";
    public const string PaymentSpecCashJson = $"{PaymentSpecCash}+json; charset=utf-8; q=1";
    public const string PaymentSpecBankJson = $"{PaymentSpecBank}+json; charset=utf-8; q=1";
    public const string PaymentSpecCryptoJson = $"{PaymentSpecCrypto}+json; charset=utf-8; q=1";
}
