namespace OnlineMinion.Application.Contracts.PaymentSpecShared.Responses;

public record PaymentSpecCashResp(
    Guid    Id,
    string  Name,
    string  CurrencyCode,
    string? Tags
) : BasePaymentSpecResp(Id, Name, CurrencyCode, Tags);
