using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.AccountSpec.Requests;

public record CheckAccountSpecUniqueExistingReq(string MemberValue, Guid OwnId)
    : ICheckUniqueExistingModelByMemberRequest;
