using OnlineMinion.Contracts.AppMessaging;

namespace OnlineMinion.Web.AppMessaging.Requests;

public abstract class BaseUpsertAccountSpecReq : BaseUpsertAccountSpecReqData, IsSuccessResultRequest
{
    protected BaseUpsertAccountSpecReq(string name, string group, string? description) :
        base(name, group, description) { }
}
