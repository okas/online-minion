using System.ComponentModel.DataAnnotations;
using MediatR;
using OnlineMinion.Contracts.Responses;

namespace OnlineMinion.Contracts.AppMessaging.Requests;

/// <param name="Filter">
///     Filter string, see:
///     <a href="https://dynamic-linq.net/basic-query-operators">Dynamic Linq syntax</a>
/// </param>
/// <param name="Sort">
///     Sort/OrderBy string, see:
///     <a href="https://dynamic-linq.net/basic-query-operators">Dynamic Linq syntax</a>
/// </param>
public record GetAccountSpecsReq(
    string?             Filter = default,
    string?             Sort   = default,
    [Range(1, 50)]  int Page   = 1,
    [Range(1, 100)] int Size   = 10
) : IRequest<PagedResult<AccountSpecResp>>, IQueryParams;
