using System.ComponentModel.DataAnnotations;

namespace OnlineMinion.Contracts.AppMessaging;

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

    [Required(DisallowAllDefaultValues = true)]
    public string Name { get; set; }

    [Required(DisallowAllDefaultValues = true)]
    public string Group { get; set; }

    public string? Description { get; set; }
}
