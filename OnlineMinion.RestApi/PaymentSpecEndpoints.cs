using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OnlineMinion.Application.Contracts;
using OnlineMinion.Application.Contracts.PaymentSpecBank.Requests;
using OnlineMinion.Application.Contracts.PaymentSpecCash.Requests;
using OnlineMinion.Application.Contracts.PaymentSpecCrypto.Requests;
using OnlineMinion.Application.Contracts.PaymentSpecShared;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Responses;
using OnlineMinion.RestApi.Helpers;
using OnlineMinion.RestApi.Services.LinkGeneration;
using static OnlineMinion.RestApi.ICommonValidationEndpoints;
using static OnlineMinion.RestApi.Configuration.ApiCorsOptionsConfigurator;
using static OnlineMinion.RestApi.ICommonDescriptorEndpoints;

namespace OnlineMinion.RestApi;

public class PaymentSpecEndpoints
    : ICommonCrudEndpoints, ICommonDescriptorEndpoints, ICommonPagingInfoEndpoints, ICommonValidationEndpoints
{
    public const string V1GetPaymentSpecById = nameof(V1GetPaymentSpecById);

    public static void MapAll(IEndpointRouteBuilder app)
    {
        var apiV1 = app.NewVersionedApi("Payment Specs")
            .MapGroup("api/payment-specs")
            .HasApiVersion(1)
            .MapToApiVersion(1);

        var linkGeneratorMetaData = new ResourceLinkGeneratorMetaData(V1GetPaymentSpecById);

        #region Common & -CRUD endpoints

        apiV1.MapGet("/", ICommonCrudEndpoints.GetSomePaged<PaymentSpecResp>)
            .RequireCors(ExposedHeadersPagingMetaInfoPolicy);

        apiV1.MapGet("{id:guid}", ICommonCrudEndpoints.GetById<GetByIdPaymentSpecReq, BasePaymentSpecResp>)
            .WithName(V1GetPaymentSpecById)
            .WithMetadata(linkGeneratorMetaData)
            .AddEndpointFilter<PaymentSpecResultFilter>()
            .Produces<BasePaymentSpecResp>(
                StatusCodes.Status200OK,
                CustomVendorContentTypes.PaymentSpecCashJson,
                CustomVendorContentTypes.PaymentSpecBankJson,
                CustomVendorContentTypes.PaymentSpecCryptoJson
            );

        apiV1.MapDelete("{id:guid}", ICommonCrudEndpoints.Delete<DeletePaymentSpecReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapHead("/", ICommonPagingInfoEndpoints.GetPagingMetaInfo<GetPaymentSpecPagingMetaInfoReq>)
            .RequireCors(ExposedHeadersPagingMetaInfoPolicy);

        apiV1.MapGet($"{DescriptorsCommonRoute}", GetAsDescriptors<PaymentSpecDescriptorResp>);

        apiV1.MapHead($"{NewNameValidationRoute}", CheckUniqueNew<CheckPaymentSpecUniqueNewReq>);

        apiV1.MapHead($"{ExistingNameValidationRoute}", CheckUniqueExisting<CheckPaymentSpecUniqueExistingReq>);

        #endregion

        #region Cash endpoints

        const string cashRoute = nameof(PaymentSpecType.Cash);

        apiV1.MapPost($"{cashRoute}", ICommonCrudEndpoints.Create<CreatePaymentSpecCashReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapPut($@"{cashRoute}/{{id:guid}}", ICommonCrudEndpoints.Update<UpdatePaymentSpecCashReq>)
            .WithMetadata(linkGeneratorMetaData)
            .Accepts<UpdatePaymentSpecCashReq>(CustomVendorContentTypes.PaymentSpecCashJson);

        #endregion

        #region Bank endpoints

        const string bankRoute = nameof(PaymentSpecType.Bank);

        apiV1.MapPost($"{bankRoute}", ICommonCrudEndpoints.Create<CreatePaymentSpecBankReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapPut($@"{bankRoute}/{{id:guid}}", ICommonCrudEndpoints.Update<UpdatePaymentSpecBankReq>)
            .WithMetadata(linkGeneratorMetaData)
            .Accepts<UpdatePaymentSpecBankReq>(CustomVendorContentTypes.PaymentSpecBankJson);

        #endregion

        #region Crypto endpoints

        const string cryptoRoute = nameof(PaymentSpecType.Crypto);

        apiV1.MapPost($"{cryptoRoute}", ICommonCrudEndpoints.Create<CreatePaymentSpecCryptoReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapPut($@"{cryptoRoute}/{{id:guid}}", ICommonCrudEndpoints.Update<UpdatePaymentSpecCryptoReq>)
            .WithMetadata(linkGeneratorMetaData)
            .Accepts<UpdatePaymentSpecCryptoReq>(CustomVendorContentTypes.PaymentSpecCryptoJson);

        #endregion

        // TODO: fix FE project handler URLs!
    }
}
