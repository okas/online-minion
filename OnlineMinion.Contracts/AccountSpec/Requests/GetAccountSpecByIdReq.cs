using System.ComponentModel.DataAnnotations;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.Contracts.AccountSpec.Requests;

public record GetAccountSpecByIdReq([Required] int Id) : IGetByIdRequest<AccountSpecResp?>;
