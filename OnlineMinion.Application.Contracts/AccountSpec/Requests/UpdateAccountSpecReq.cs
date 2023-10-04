using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.AccountSpec.Requests;

public sealed class UpdateAccountSpecReq(Guid id, string name, string group, string? description)
    : BaseUpsertAccountSpecReqData(name, group, description), IUpdateCommand
{
    public UpdateAccountSpecReq() : this(Guid.Empty, string.Empty, string.Empty, null) { }

    public Guid Id { get; set; } = id;
}
