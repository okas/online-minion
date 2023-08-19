using System.ComponentModel.DataAnnotations;
using MediatR;
using OnlineMinion.Contracts.Responses;

namespace OnlineMinion.Contracts.AppMessaging;

/// <param name="Filter">
///     Filter string <a href="https://dynamic-linq.net/basic-simple-query#more-where-examples">see docs</a>.
/// </param>
/// <param name="Sort">
///     Sort/OrderBy string <a href="https://dynamic-linq.net/basic-simple-query#ordering-results">see docs</a>.
/// </param>
public record BaseGetSomeReq<TResp>(
    string?             Filter = default,
    string?             Sort   = default,
    [Range(1, 50)]  int Page   = 1,
    [Range(1, 100)] int Size   = 10
) : IRequest<PagedResult<TResp>>, IQueryParams;
