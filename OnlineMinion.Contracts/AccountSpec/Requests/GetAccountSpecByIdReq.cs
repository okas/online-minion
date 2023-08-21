using System.ComponentModel.DataAnnotations;
using MediatR;
using OnlineMinion.Contracts.AccountSpec.Responses;

namespace OnlineMinion.Contracts.AccountSpec.Requests;

public record GetAccountSpecByIdReq([Required] int Id) : IHasIntId, IRequest<AccountSpecResp?>;
