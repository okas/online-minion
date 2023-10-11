using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using ErrorOr;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Application.Contracts;
using OnlineMinion.Application.Contracts.PaymentSpecShared;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Responses;
using OnlineMinion.Application.Contracts.Shared.Requests;
using OnlineMinion.Application.Contracts.Shared.Responses;
using OnlineMinion.Application.Paging;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Application.PaymentSpecSharedHandlers;

[UsedImplicitly]
internal sealed partial class GetSomePaymentSpecsReqHlr(IOnlineMinionDbContext dbContext)
    : IErrorOrRequestHandler<GetSomeModelsPagedReq<PaymentSpecResp>, PagedStreamResult<PaymentSpecResp>>
{
    private const string CapturingGroupName = "type";

    public async Task<ErrorOr<PagedStreamResult<PaymentSpecResp>>> Handle(
        GetSomeModelsPagedReq<PaymentSpecResp> rq,
        CancellationToken                      ct
    )
    {
        var query = dbContext.Set<BasePaymentSpecData>().AsNoTracking();

        var pagingMeta = await query.GetPagingMetaInfoAsync(rq, ct).ConfigureAwait(false);

        var resultStream = CreateStoreQuery(query, rq).AsAsyncEnumerable();

        return new PagedStreamResult<PaymentSpecResp>(resultStream, pagingMeta);
    }

    private static IQueryable<PaymentSpecResp> CreateStoreQuery(
        IQueryable<BasePaymentSpecData> query,
        IQueryParameters                queryParams
    )
    {
        if (!string.IsNullOrWhiteSpace(queryParams.Filter))
        {
            query = query.Where(GetFixedFilter(queryParams.Filter));
        }

        var letProjectionQuery = query.Select(
            entity => new
            {
                entity.Id,
                entity.Name,
                entity.CurrencyCode,
                entity.Tags,
                // let keyword similar behavior, needed to be able to sort by derived type semantics
                // Name is important, for simplicity it must be same as nameof(PaymentSpecResp.Type)!
                Type = entity is PaymentSpecCash
                    ? PaymentSpecType.Cash
                    : entity is PaymentSpecBank
                        ? PaymentSpecType.Bank
                        : PaymentSpecType.Crypto,
            }
        );

        var orderedQuery = !string.IsNullOrWhiteSpace(queryParams.Sort)
            ? letProjectionQuery.OrderBy(queryParams.Sort)
            : letProjectionQuery.OrderBy(x => x.Id);

        var resultProjectionQuery = orderedQuery.Select(
            x => new PaymentSpecResp(
                x.Id.Value,
                x.Name,
                x.CurrencyCode,
                x.Tags,
                x.Type
            )
        );

        return resultProjectionQuery.Page(queryParams.Page, queryParams.Size);
    }

    /// <summary>
    ///     Handles DynamicLinq filtering matters, especially where <see cref="PaymentSpecType" /> is used.
    /// </summary>
    /// <param name="dynamicLinqFilterExpression">"Raw", input DynamicLinq expression.</param>
    /// <returns>
    ///     Guaranteed working DynamicLinq expression that conditionally have replaced enum semantics of
    ///     <see cref="PaymentSpecResp.Type" /> to entity type comparison according to derived types of
    ///     <see cref="BasePaymentSpecData" />.
    /// </returns>
    /// <remarks>
    ///     <see cref="PaymentSpecType" /> can be used in filter, but it is not a property of
    ///     <see cref="BasePaymentSpecData" />,
    ///     so it is not possible to use it directly in DynamicLinq's
    ///     <see cref="DynamicQueryableExtensions.Where{TSource}(System.Linq.IQueryable{TSource},string,object?[])" /> method.
    ///     To overcome this, we use
    ///     <see cref="PaymentSpecTypeRegEx" /> to match enum value in <see cref="IQueryParameters.Filter" />, then we use this
    ///     value to
    ///     get the name of derived type of <see cref="BasePaymentSpecData" /> and finally we use this name in
    ///     DynamicLinq's "is" operator to match entity with derived type.
    /// </remarks>
    private static string GetFixedFilter(string dynamicLinqFilterExpression)
    {
        var match = PaymentSpecTypeRegEx().Match(dynamicLinqFilterExpression);

        // If no match was found, then Type property is not used in filter, return input query.
        if (!match.Success)
        {
            return dynamicLinqFilterExpression;
        }

        var enumValue = match.Groups[CapturingGroupName].Value;
        var paymentSpecType = Enum.Parse<PaymentSpecType>(enumValue);

        // Using found paymentSpecType, get the name of derived type of BasePaymentSpecData.
        var paymentSpecTypeName = paymentSpecType switch
        {
            PaymentSpecType.Cash => typeof(PaymentSpecCash).FullName,
            PaymentSpecType.Bank => typeof(PaymentSpecBank).FullName,
            _ => typeof(PaymentSpecCrypto).FullName,
        };

        // Using PaymentSpecTypeRegEx, replace found value with call to DynamicLinq's "is" operator expression.
        var replacement = $"Is(\"{paymentSpecTypeName}\")";
        var fixedFilter = PaymentSpecTypeRegEx().Replace(dynamicLinqFilterExpression, replacement);

        return fixedFilter;
    }

    /// <summary>
    ///     Create regexp using $"@{nameof(PaymentSpecResp.Type)} = enumValue" to match one of enum value in named group.
    /// </summary>
    [GeneratedRegex($@"@{nameof(PaymentSpecResp.Type)}\s*=\s*(?<{CapturingGroupName}>[0-2]{{1}})\s*")]
    private static partial Regex PaymentSpecTypeRegEx();
}
