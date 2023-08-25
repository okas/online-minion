using System.ComponentModel.DataAnnotations;
using MediatR;
using OnlineMinion.Contracts.Shared.Responses;

namespace OnlineMinion.Contracts.Shared.Requests;

/// <param name="Filter">
///     Filter string <a href="https://dynamic-linq.net/basic-simple-query#more-where-examples">see docs</a>.
/// </param>
/// <param name="Sort">
///     Sort/OrderBy string <a href="https://dynamic-linq.net/basic-simple-query#ordering-results">see docs</a>.
/// </param>
public record BaseGetSomePagedReq<TResp>(
    string?             Filter = default,
    string?             Sort   = default,
    [Range(1, 50)]  int Page   = 1,
    [Range(1, 100)] int Size   = 10
) : IRequest<PagedResult<TResp>>, IQueryParams;
