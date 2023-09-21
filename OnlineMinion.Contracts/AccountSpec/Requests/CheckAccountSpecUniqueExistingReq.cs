using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.Contracts.AccountSpec.Requests;

public record CheckAccountSpecUniqueExistingReq(string MemberValue, int OwnId)
    : ICheckUniqueExistingModelByMemberRequest;
