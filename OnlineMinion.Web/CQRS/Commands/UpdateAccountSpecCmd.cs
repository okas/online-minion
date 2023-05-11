namespace OnlineMinion.Web.CQRS.Commands;

public sealed class UpdateAccountSpecCmd : BaseUpsertAccountSpecCmd
{
    public UpdateAccountSpecCmd() : base(string.Empty, string.Empty, null) { }

    public UpdateAccountSpecCmd(int id, string name, string group, string? description) :
        base(name, group, description) =>
        Id = id;

    public int Id { get; set; }
}
