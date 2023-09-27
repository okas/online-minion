namespace OnlineMinion.Contracts.AccountSpec.Responses;

public record struct AccountSpecResp(Guid Id, string Name, string Group, string? Description) : IHasId;
