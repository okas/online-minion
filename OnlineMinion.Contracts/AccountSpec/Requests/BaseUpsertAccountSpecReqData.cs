namespace OnlineMinion.Contracts.AccountSpec.Requests;

public abstract class BaseUpsertAccountSpecReqData
{
    protected BaseUpsertAccountSpecReqData(string name, string group, string? description)
    {
        Name = name;
        Group = group;
        Description = description;
    }

    public string Name { get; set; }

    public string Group { get; set; }

    public string? Description { get; set; }
}
