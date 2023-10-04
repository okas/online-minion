namespace OnlineMinion.Application.Contracts.PaymentSpecShared.Responses;

public record struct PaymentSpecResp(Guid Id, string Name, string CurrencyCode, string? Tags) : IHasId;
