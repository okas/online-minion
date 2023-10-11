namespace OnlineMinion.Application.Contracts.PaymentSpecShared.Responses;

public record PaymentSpecBankResp(
    Guid    Id,
    string  Name,
    string  CurrencyCode,
    string? Tags,
    string  IBAN
) : BasePaymentSpecResp(Id, Name, CurrencyCode, Tags);
