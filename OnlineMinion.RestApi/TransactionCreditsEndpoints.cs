using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Contracts.Transactions.Credit.Responses;
using OnlineMinion.RestApi.Helpers;
using OnlineMinion.RestApi.Paging;
using OnlineMinion.RestApi.Services;
using OnlineMinion.RestApi.Shared.Endpoints;
using static OnlineMinion.RestApi.Configuration.ApiCorsOptionsConfigurator;
using static OnlineMinion.RestApi.Shared.NamedRoutes;

namespace OnlineMinion.RestApi;

public class TransactionCreditsEndpoints : ICRUDEndpoints, ICommonPagingInfoEndpoints
{
    public static void MapAll(IEndpointRouteBuilder app)
    {
        var apiV1 = app.NewVersionedApi("Credit Transactions")
            .MapGroup("api/transactions/credits")
            .HasApiVersion(1)
            .MapToApiVersion(1);

        var linkGeneratorMetaData = new LinkGeneratorMetaData(V1GetTransactCreditById);

        apiV1.MapPost("/", ICRUDEndpoints.Create<CreateTransactionCreditReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapGet("/", ICRUDEndpoints.GetSomePaged<TransactionCreditResp>)
            .RequireCors(ExposedHeadersPagingMetaInfoPolicy);

        apiV1.MapGet("{id:int}", ICRUDEndpoints.GetById<GetTransactionCreditByIdReq, TransactionCreditResp>)
            .WithName(V1GetTransactCreditById)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapPut("{id:int}", ICRUDEndpoints.Update<UpdateTransactionCreditReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapDelete("{id:int}", ICRUDEndpoints.Delete<DeleteTransactionCreditReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapHead("/", ICommonPagingInfoEndpoints.GetPagingMetaInfo<GetTransactionCreditPagingMetaInfoReq>)
            .RequireCors(ExposedHeadersPagingMetaInfoPolicy);
    }
}
