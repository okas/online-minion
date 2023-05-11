using System.ComponentModel.DataAnnotations;

namespace OnlineMinion.Contracts.Commands;

// TODO: to ref record?
/// <summary>
///     Base class, that provides some model metadata.
/// </summary>
public abstract class BaseUpsertAccountSpecCmdData
{
    protected BaseUpsertAccountSpecCmdData(string name, string group, string? description)
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
