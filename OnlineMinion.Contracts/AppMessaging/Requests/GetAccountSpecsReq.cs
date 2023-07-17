using System.ComponentModel.DataAnnotations;
using MediatR;
using OnlineMinion.Contracts.Responses;

namespace OnlineMinion.Contracts.AppMessaging.Requests;

public record GetAccountSpecsReq(
    string?             Filter = default,
    string?             Sort   = default,
    [Range(1, 50)]  int Page   = 1,
    [Range(1, 100)] int Size   = 10
) : IRequest<PagedResult<AccountSpecResp>>, IQueryParams;
