namespace OnlineMinion.Contracts.PaymentSpec.Responses;

public record struct PaymentSpecDescriptorResp(int Id, string Name) : IHasIntId;
