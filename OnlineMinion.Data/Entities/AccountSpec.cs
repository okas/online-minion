using System.Diagnostics.CodeAnalysis;

namespace OnlineMinion.Data.Entities;

public class AccountSpec : BaseEntity
{
    public AccountSpec() { }

    [SetsRequiredMembers]
    public AccountSpec(string name, string group, string? description)
    {
        Name = name;
        Group = group;
        Description = description;
    }

    public required string Name { get; set; }

    public required string Group { get; set; }

    public string? Description { get; set; }
}
