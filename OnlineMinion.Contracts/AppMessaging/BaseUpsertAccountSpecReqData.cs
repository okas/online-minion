namespace OnlineMinion.Contracts.AppMessaging;

/// <summary>
///     Base class, that provides some model metadata.
/// </summary>
public abstract class BaseUpsertAccountSpecReqData : ICommand
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
