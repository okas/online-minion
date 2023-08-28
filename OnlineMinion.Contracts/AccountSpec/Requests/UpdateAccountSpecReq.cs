using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.Contracts.AccountSpec.Requests;

public sealed class UpdateAccountSpecReq : BaseUpsertAccountSpecReqData, IUpdateCommand
{
    public UpdateAccountSpecReq() : base(string.Empty, string.Empty, null) { }

    public UpdateAccountSpecReq(int id, string name, string group, string? description) :
        base(name, group, description) =>
        Id = id;

    public int Id { get; set; }

    public static implicit operator UpdateAccountSpecReq(AccountSpecResp resp) =>
        new(resp.Id, resp.Name, resp.Group, resp.Description);

    public static explicit operator AccountSpecResp(UpdateAccountSpecReq rq) =>
        new(rq.Id, rq.Name, rq.Group, rq.Description);
}
