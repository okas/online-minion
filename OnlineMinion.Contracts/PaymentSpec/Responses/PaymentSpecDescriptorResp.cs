namespace OnlineMinion.Contracts.PaymentSpec.Responses;

public record struct PaymentSpecDescriptorResp(Guid Id, string Name, string CurrencyCode) : IHasId;
