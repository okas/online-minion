using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.Contracts.AccountSpec.Requests;

[UsedImplicitly]
public record GetAccountSpecByIdReq([Required] Guid Id) : IGetByIdRequest<AccountSpecResp>;
