namespace OnlineMinion.Contracts.PaymentSpec.Responses;

public record struct PaymentSpecResp(int Id, string Name, string CurrencyCode, string? Tags) : IHasId;
