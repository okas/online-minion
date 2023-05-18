using System.ComponentModel.DataAnnotations;
using MediatR;

namespace OnlineMinion.Contracts.AppMessaging.Requests;

public sealed class UpdateAccountSpecReq : BaseUpsertAccountSpecReqData, IRequest<bool>
{
    public UpdateAccountSpecReq() : base(string.Empty, string.Empty, null) { }

    public UpdateAccountSpecReq(int id, string name, string group, string? description) :
        base(name, group, description) =>
        Id = id;

    [Required]
    public int Id { get; set; }
}
