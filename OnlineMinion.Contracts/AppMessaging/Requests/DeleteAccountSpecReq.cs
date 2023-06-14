using System.ComponentModel.DataAnnotations;
using MediatR;

namespace OnlineMinion.Contracts.AppMessaging.Requests;

public record DeleteAccountSpecReq([Required] int Id) : IHasIntId, IRequest<bool>;
