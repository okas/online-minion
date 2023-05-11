namespace OnlineMinion.Web.CQRS.Commands;

public sealed class CreateAccountSpecCmd : BaseUpsertAccountSpecCmd
{
    public CreateAccountSpecCmd() : base(string.Empty, string.Empty, null) { }

    public CreateAccountSpecCmd(string name, string group, string? description) : base(name, group, description) { }
}
