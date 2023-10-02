using System.Diagnostics.CodeAnalysis;

namespace OnlineMinion.Domain.AccountSpecs;

[method: SetsRequiredMembers]
public sealed class AccountSpec(string name, string group, string? description) : IEntity<AccountSpecId>
{
    public required string Name { get; set; } = name;

    public required string Group { get; set; } = group;

    public string? Description { get; set; } = description;

    public AccountSpecId Id { get; set; } = new();
}
