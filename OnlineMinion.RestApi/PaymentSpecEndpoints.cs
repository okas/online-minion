using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OnlineMinion.Application.Contracts;
using OnlineMinion.Application.Contracts.PaymentSpecBank.Requests;
using OnlineMinion.Application.Contracts.PaymentSpecCash.Requests;
using OnlineMinion.Application.Contracts.PaymentSpecCrypto.Requests;
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

        #region Common & common CRUD endpoints

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

        apiV1.MapHead($"{NewNameValidationRoute}", CheckUniqueByMember<CheckUniqueNewPaymentSpecNameReq>);

        apiV1.MapHead($"{ExistingNameValidationRoute}", CheckUniqueByMember<CheckUniqueExistingPaymentSpecNameReq>);

        #endregion

        #region Cash endpoints

        apiV1.MapPost("cash", ICommonCrudEndpoints.Create<CreatePaymentSpecCashReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapPut(@"cash/{id:guid}", ICommonCrudEndpoints.Update<UpdatePaymentSpecCashReq>)
            .WithMetadata(linkGeneratorMetaData)
            .Accepts<UpdatePaymentSpecCashReq>(CustomVendorContentTypes.PaymentSpecCashJson);

        #endregion

        #region Bank endpoints

        apiV1.MapPost("bank", ICommonCrudEndpoints.Create<CreatePaymentSpecBankReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapPut(@"bank/{id:guid}", ICommonCrudEndpoints.Update<UpdatePaymentSpecBankReq>)
            .WithMetadata(linkGeneratorMetaData)
            .Accepts<UpdatePaymentSpecBankReq>(CustomVendorContentTypes.PaymentSpecBankJson);

        apiV1.MapHead(
            $"bank/{NewIBANValidationRoute}",
            CheckUniqueByMember<CheckUniqueNewPaymentSpecBankIBANReq>
        );

        apiV1.MapHead(
            $"bank/{ExistingIBANValidationRoute}",
            CheckUniqueByMember<CheckUniqueExistingPaymentSpecBankIBANReq>
        );

        #endregion

        #region Crypto endpoints

        apiV1.MapPost("crypto", ICommonCrudEndpoints.Create<CreatePaymentSpecCryptoReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapPut(@"crypto/{id:guid}", ICommonCrudEndpoints.Update<UpdatePaymentSpecCryptoReq>)
            .WithMetadata(linkGeneratorMetaData)
            .Accepts<UpdatePaymentSpecCryptoReq>(CustomVendorContentTypes.PaymentSpecCryptoJson);

        #endregion

        // TODO: fix FE project handler URLs!
    }
}
