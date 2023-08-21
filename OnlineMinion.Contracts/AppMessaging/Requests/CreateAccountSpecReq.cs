using OnlineMinion.Contracts.Responses;

namespace OnlineMinion.Contracts.AppMessaging.Requests;

public sealed class CreateAccountSpecReq : BaseUpsertAccountSpecReqData, ICreateCommand
{
    public CreateAccountSpecReq() : base(string.Empty, string.Empty, null) { }

    public CreateAccountSpecReq(string name, string group, string? description) : base(name, group, description) { }

    public AccountSpecResp ToResponse(int id) => new(id, Name, Group, Description);
}
