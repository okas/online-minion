namespace OnlineMinion.Application.Contracts.PaymentSpecShared.Responses;

public abstract record BasePaymentSpecResp(
    Guid    Id,
    string  Name,
    string  CurrencyCode,
    string? Tags
) : IHasId;
