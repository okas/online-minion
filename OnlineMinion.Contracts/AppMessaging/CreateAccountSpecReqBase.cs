using System.ComponentModel.DataAnnotations;

namespace OnlineMinion.Contracts.AppMessaging;

// TODO: to ref record?
/// <summary>
///     Base class, that provides some model metadata.
/// </summary>
public abstract class BaseUpsertAccountSpecReqData
{
    protected BaseUpsertAccountSpecReqData(string name, string group, string? description)
    {
        Name = name;
        Group = group;
        Description = description;
    }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Group { get; set; }

    public string? Description { get; set; }
}
