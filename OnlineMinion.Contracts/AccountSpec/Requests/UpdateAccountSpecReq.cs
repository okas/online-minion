using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.Contracts.AccountSpec.Requests;

public sealed class UpdateAccountSpecReq(Guid id, string name, string group, string? description)
    : BaseUpsertAccountSpecReqData(name, group, description), IUpdateCommand
{
    public UpdateAccountSpecReq() : this(Guid.Empty, string.Empty, string.Empty, null) { }

    public Guid Id { get; set; } = id;

    public static implicit operator UpdateAccountSpecReq(AccountSpecResp resp) =>
        new(resp.Id, resp.Name, resp.Group, resp.Description);

    public static explicit operator AccountSpecResp(UpdateAccountSpecReq rq) =>
        new(rq.Id, rq.Name, rq.Group, rq.Description);
}
