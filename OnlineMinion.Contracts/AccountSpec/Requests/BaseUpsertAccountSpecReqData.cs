namespace OnlineMinion.Contracts.AccountSpec.Requests;

public abstract class BaseUpsertAccountSpecReqData(string name, string group, string? description)
{
    public string Name { get; set; } = name;

    public string Group { get; set; } = group;

    public string? Description { get; set; } = description;
}
