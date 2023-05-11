using OnlineMinion.Contracts.Commands;
using OnlineMinion.Contracts.Responses;

namespace OnlineMinion.Web.CQRS.Commands;

public abstract class BaseUpsertAccountSpecCmd : BaseUpsertAccountSpecCmdData, IsSuccessResultCommand
{
    protected BaseUpsertAccountSpecCmd(string name, string group, string? description) :
        base(name, group, description) { }
}
