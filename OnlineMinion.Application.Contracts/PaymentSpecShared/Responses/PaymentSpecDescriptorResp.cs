namespace OnlineMinion.Application.Contracts.PaymentSpecShared.Responses;

public record struct PaymentSpecDescriptorResp(Guid Id, string Name, string CurrencyCode) : IHasId;
