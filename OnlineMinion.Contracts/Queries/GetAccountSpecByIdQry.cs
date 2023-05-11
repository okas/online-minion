using System.ComponentModel.DataAnnotations;
using MediatR;
using OnlineMinion.Contracts.Responses;

namespace OnlineMinion.Contracts.Queries;

public record GetAccountSpecByIdQry([Required] int Id) : IRequest<AccountSpecResp?>;
