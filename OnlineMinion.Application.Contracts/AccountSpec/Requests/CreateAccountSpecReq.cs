using OnlineMinion.Application.Contracts.AccountSpec.Responses;
using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.AccountSpec.Requests;

public sealed class CreateAccountSpecReq(string name, string group, string? description)
    : BaseUpsertAccountSpecReqData(name, group, description), ICreateCommand
{
    public CreateAccountSpecReq() : this(string.Empty, string.Empty, null) { }

    public AccountSpecResp ToResponse(Guid id) => new(id, Name, Group, Description);
}
