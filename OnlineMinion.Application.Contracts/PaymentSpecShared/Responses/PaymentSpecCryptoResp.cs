namespace OnlineMinion.Application.Contracts.PaymentSpecShared.Responses;

public record PaymentSpecCryptoResp(
    Guid    Id,
    string  Name,
    string  CurrencyCode,
    string? Tags,
    string  ExchangeName,
    bool    IsFiat
) : BasePaymentSpecResp(Id, Name, CurrencyCode, Tags);
