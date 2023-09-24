using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.RestApi.Helpers;
using OnlineMinion.RestApi.Paging;
using OnlineMinion.RestApi.Services;
using OnlineMinion.RestApi.Shared.Endpoints;
using static OnlineMinion.RestApi.Shared.CommonValidationEndpoints;
using static OnlineMinion.RestApi.Configuration.ApiCorsOptionsConfigurator;
using static OnlineMinion.RestApi.Shared.NamedRoutes;

namespace OnlineMinion.RestApi;

public class PaymentSpecsEndpoints : ICRUDEndpoints
{
    public static void MapAll(IEndpointRouteBuilder app)
    {
        var apiV1 = app.NewVersionedApi("Payment Specs")
            .MapGroup("api/payment-specs")
            .HasApiVersion(1)
            .MapToApiVersion(1);

        var linkGeneratorMetaData = new LinkGeneratorMetaData(V1GetPaymentSpecById);

        apiV1.MapPost("/", ICRUDEndpoints.Create<CreatePaymentSpecReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapGet("/", ICRUDEndpoints.GetSomePaged<PaymentSpecResp>)
            .RequireCors(ExposedHeadersPagingMetaInfoPolicy);

        apiV1.MapGet("{id:int}", ICRUDEndpoints.GetById<GetPaymentSpecByIdReq, PaymentSpecResp>)
            .WithName(V1GetPaymentSpecById)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapPut("{id:int}", ICRUDEndpoints.Update<UpdatePaymentSpecReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapDelete("{id:int}", ICRUDEndpoints.Delete<DeletePaymentSpecReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapHead("/", CommonPagingInfoEndpoints.GetPagingMetaInfo<GetPaymentSpecPagingMetaInfoReq>)
            .RequireCors(ExposedHeadersPagingMetaInfoPolicy);

        apiV1.MapGet("descriptors", ICRUDEndpoints.GetAsDescriptors<PaymentSpecDescriptorResp>);

        apiV1.MapHead($"{NewNameValidationRoute}", CheckUniqueNew<CheckPaymentSpecUniqueNewReq>);

        apiV1.MapHead($"{ExistingNameValidationRoute}", CheckUniqueExisting<CheckPaymentSpecUniqueExistingReq>);
    }
}
