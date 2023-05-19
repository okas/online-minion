namespace OnlineMinion.RestApi.Client.Requests;

public sealed class UpdateAccountSpecReq : BaseUpsertAccountSpecReq
{
    public UpdateAccountSpecReq() : base(string.Empty, string.Empty, null) { }

    public UpdateAccountSpecReq(int id, string name, string group, string? description) :
        base(name, group, description) =>
        Id = id;

    public int Id { get; set; }
}
