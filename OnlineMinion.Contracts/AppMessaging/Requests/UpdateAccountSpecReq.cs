using System.ComponentModel.DataAnnotations;
using ErrorOr;
using MediatR;

namespace OnlineMinion.Contracts.AppMessaging.Requests;

public sealed class UpdateAccountSpecReq : BaseUpsertAccountSpecReqData, IHasIntId, IRequest<ErrorOr<bool>>
{
    public UpdateAccountSpecReq() : base(string.Empty, string.Empty, null) { }

    public UpdateAccountSpecReq(int id, string name, string group, string? description) :
        base(name, group, description) =>
        Id = id;

    [Required(DisallowAllDefaultValues = true)]
    public int Id { get; set; }
}
