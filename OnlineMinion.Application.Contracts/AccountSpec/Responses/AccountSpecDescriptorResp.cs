namespace OnlineMinion.Application.Contracts.AccountSpec.Responses;

public record struct AccountSpecDescriptorResp(Guid Id, string Name) : IHasId;
