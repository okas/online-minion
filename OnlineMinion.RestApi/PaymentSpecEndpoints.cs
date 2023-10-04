using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using OnlineMinion.Application.Contracts.PaymentSpecCash.Requests;
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

        apiV1.MapGet("/", ICommonCrudEndpoints.GetSomePaged<PaymentSpecResp>)
            .RequireCors(ExposedHeadersPagingMetaInfoPolicy);

        apiV1.MapGet("{id:guid}", ICommonCrudEndpoints.GetById<GetPaymentSpecByIdReq, PaymentSpecResp>)
            .WithName(V1GetPaymentSpecById)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapDelete("{id:guid}", ICommonCrudEndpoints.Delete<DeletePaymentSpecReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapHead("/", ICommonPagingInfoEndpoints.GetPagingMetaInfo<GetPaymentSpecPagingMetaInfoReq>)
            .RequireCors(ExposedHeadersPagingMetaInfoPolicy);

        apiV1.MapGet($"{DescriptorsCommonRoute}", GetAsDescriptors<PaymentSpecDescriptorResp>);

        apiV1.MapHead($"{NewNameValidationRoute}", CheckUniqueNew<CheckPaymentSpecUniqueNewReq>);

        apiV1.MapHead($"{ExistingNameValidationRoute}", CheckUniqueExisting<CheckPaymentSpecUniqueExistingReq>);

        apiV1.MapPost("cash", ICommonCrudEndpoints.Create<CreatePaymentSpecCashReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapPut("cash/{id:guid}", ICommonCrudEndpoints.Update<UpdatePaymentSpecCashReq>)
            .WithMetadata(linkGeneratorMetaData);

        // TODO: fix FE project handler URLs!
    }
}
