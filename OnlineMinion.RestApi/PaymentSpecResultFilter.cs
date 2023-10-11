using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using OnlineMinion.Application.Contracts;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Responses;

namespace OnlineMinion.RestApi;

/// <summary>
///     Re-creates the <see cref="JsonHttpResult{TValue}" /> from initial <see cref="Ok{TValue}" /> result, but with custom
///     content type.
/// </summary>
/// <remarks>
///     For API documentation, add content type info with call to the
///     <see cref="OpenApiRouteHandlerBuilderExtensions.Produces{TResponse}" /> method.<br />
///     Depending of the implementation of the <see cref="BasePaymentSpecResp" />, that is returned as a value from the
///     endpoint, this filter will output results with content types either of
///     <see cref="CustomVendorContentTypes.PaymentSpecCashJsonCharset" />,
///     <see cref="CustomVendorContentTypes.PaymentSpecBankJsonCharset" /> or
///     <see cref="CustomVendorContentTypes.PaymentSpecCryptoJsonCharset" />.
/// </remarks>
public class PaymentSpecResultFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var endpointResult = await next.Invoke(context).ConfigureAwait(false);

        return (endpointResult as INestedHttpResult)?.Result switch
        {
            Ok<BasePaymentSpecResp> result => result.Value switch
            {
                PaymentSpecCashResp value =>
                    TypedResults.Json(
                        value,
                        statusCode: result.StatusCode,
                        contentType: CustomVendorContentTypes.PaymentSpecCashJsonCharset
                    ),

                PaymentSpecBankResp value =>
                    TypedResults.Json(
                        value,
                        statusCode: result.StatusCode,
                        contentType: CustomVendorContentTypes.PaymentSpecBankJsonCharset
                    ),

                PaymentSpecCryptoResp value =>
                    TypedResults.Json(
                        value,
                        statusCode: result.StatusCode,
                        contentType: CustomVendorContentTypes.PaymentSpecCryptoJsonCharset
                    ),

                _ => result,
            },

            _ => endpointResult,
        };
    }
}
