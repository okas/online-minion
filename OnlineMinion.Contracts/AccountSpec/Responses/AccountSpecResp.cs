namespace OnlineMinion.Contracts.AccountSpec.Responses;

public record struct AccountSpecResp(int Id, string Name, string Group, string? Description) : IHasIntId;
