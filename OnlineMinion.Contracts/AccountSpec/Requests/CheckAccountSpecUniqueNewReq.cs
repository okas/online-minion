using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.Contracts.AccountSpec.Requests;

public record CheckAccountSpecUniqueNewReq(string MemberValue) : ICheckUniqueNewModelByMemberRequest;
