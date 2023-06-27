using ErrorOr;
using MediatR;

namespace OnlineMinion.Contracts.AppMessaging.Requests;

public sealed class UpdateAccountSpecReq : BaseUpsertAccountSpecReqData, IHasIntId, IRequest<ErrorOr<Updated>>
{
    public UpdateAccountSpecReq() : base(string.Empty, string.Empty, null) { }

    public UpdateAccountSpecReq(int id, string name, string group, string? description) :
        base(name, group, description) =>
        Id = id;

    public int Id { get; set; }
}
