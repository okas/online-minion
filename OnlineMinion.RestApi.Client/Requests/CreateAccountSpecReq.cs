namespace OnlineMinion.RestApi.Client.Requests;

public sealed class CreateAccountSpecReq : BaseUpsertAccountSpecReq
{
    public CreateAccountSpecReq() : base(string.Empty, string.Empty, null) { }

    public CreateAccountSpecReq(string name, string group, string? description) : base(name, group, description) { }
}
