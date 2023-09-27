using ErrorOr;
using MediatR;

namespace OnlineMinion.Contracts.Shared.Requests;

public interface ICheckUniqueExistingModelByMemberRequest : IRequest<ErrorOr<Success>>
{
    string MemberValue { get; init; }

    Guid OwnId { get; init; }
}
