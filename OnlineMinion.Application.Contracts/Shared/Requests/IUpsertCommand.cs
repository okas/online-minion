using MediatR;

namespace OnlineMinion.Application.Contracts.Shared.Requests;

/// <summary>
///     General purpose marker interface, without any members. For <see cref="MediatR" /> command pipeline filtering.
/// </summary>
public interface IUpsertCommand<out TResponse> : IRequest<TResponse>;
