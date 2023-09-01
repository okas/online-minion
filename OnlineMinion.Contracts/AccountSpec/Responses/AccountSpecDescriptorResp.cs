namespace OnlineMinion.Contracts.AccountSpec.Responses;

public record struct AccountSpecDescriptorResp(int Id, string Name) : IHasIntId;
