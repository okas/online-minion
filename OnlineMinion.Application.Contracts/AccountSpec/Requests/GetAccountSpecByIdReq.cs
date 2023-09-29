using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.AccountSpec.Responses;
using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.AccountSpec.Requests;

[UsedImplicitly]
public record GetAccountSpecByIdReq([Required] Guid Id) : IGetByIdRequest<AccountSpecResp>;
