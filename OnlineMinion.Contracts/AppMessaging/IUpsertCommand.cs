using MediatR;

namespace OnlineMinion.Contracts.AppMessaging;

/// <summary>
///     General purpose marker interface, without any members. For <see cref="MediatR" /> command pipeline filtering.
/// </summary>
public interface IUpsertCommand : IBaseRequest { }
