using ErrorOr;
using MediatR;

namespace OnlineMinion.Application.Contracts.Shared.Requests;

public interface ICheckUniqueNewModelByMemberRequest : IRequest<ErrorOr<Success>>
{
    string MemberValue { get; init; }
}
