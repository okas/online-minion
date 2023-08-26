using MediatR;

namespace OnlineMinion.Contracts.Shared.Requests;

/// <summary>
///     General purpose marker interface, without any members. For <see cref="MediatR" /> command pipeline filtering.
/// </summary>
public interface IUpsertCommand<out TResponse> : IRequest<TResponse>;
