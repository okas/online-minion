using MediatR;

namespace OnlineMinion.Contracts.Commands;

public sealed class UpdateAccountSpecCmd : BaseUpsertAccountSpecCmdData, IRequest<bool>
{
    public UpdateAccountSpecCmd() : base(string.Empty, string.Empty, null) { }

    public UpdateAccountSpecCmd(int id, string name, string group, string? description) :
        base(name, group, description) =>
        Id = id;

    public int Id { get; set; }
}
