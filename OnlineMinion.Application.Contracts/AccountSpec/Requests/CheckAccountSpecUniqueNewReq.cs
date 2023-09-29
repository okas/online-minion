using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.AccountSpec.Requests;

public record CheckAccountSpecUniqueNewReq(string MemberValue) : ICheckUniqueNewModelByMemberRequest;
