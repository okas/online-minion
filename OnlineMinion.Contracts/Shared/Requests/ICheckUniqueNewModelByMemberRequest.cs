using ErrorOr;
using MediatR;

namespace OnlineMinion.Contracts.Shared.Requests;

public interface ICheckUniqueNewModelByMemberRequest : IRequest<ErrorOr<Success>>
{
    string MemberValue { get; init; }
}
