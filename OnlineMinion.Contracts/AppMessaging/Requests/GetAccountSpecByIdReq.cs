using System.ComponentModel.DataAnnotations;
using MediatR;
using OnlineMinion.Contracts.Responses;

namespace OnlineMinion.Contracts.AppMessaging.Requests;

public record GetAccountSpecByIdReq([Required] int Id) : IRequest<AccountSpecResp?>;
