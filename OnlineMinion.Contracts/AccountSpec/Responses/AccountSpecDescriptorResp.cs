namespace OnlineMinion.Contracts.AccountSpec.Responses;

public record struct AccountSpecDescriptorResp(Guid Id, string Name) : IHasId;
