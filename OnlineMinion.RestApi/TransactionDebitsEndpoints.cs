using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using OnlineMinion.Contracts.Transactions.Debit.Requests;
using OnlineMinion.Contracts.Transactions.Debit.Responses;
using OnlineMinion.RestApi.Helpers;
using OnlineMinion.RestApi.Paging;
using OnlineMinion.RestApi.Services;
using OnlineMinion.RestApi.Shared.Endpoints;
using static OnlineMinion.RestApi.Configuration.ApiCorsOptionsConfigurator;
using static OnlineMinion.RestApi.Shared.NamedRoutes;

namespace OnlineMinion.RestApi;

public class TransactionDebitsEndpoints : ICRUDEndpoints, ICommonPagingInfoEndpoints
{
    public static void MapAll(IEndpointRouteBuilder app)
    {
        var apiV1 = app.NewVersionedApi("Debit Transactions")
            .MapGroup("api/transactions/debits")
            .HasApiVersion(1)
            .MapToApiVersion(1);

        var linkGeneratorMetaData = new LinkGeneratorMetaData(V1GetTransactDebitById);

        apiV1.MapPost("/", ICRUDEndpoints.Create<CreateTransactionDebitReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapGet("/", ICRUDEndpoints.GetSomePaged<TransactionDebitResp>)
            .RequireCors(ExposedHeadersPagingMetaInfoPolicy);

        apiV1.MapGet("{id:int}", ICRUDEndpoints.GetById<GetTransactionDebitByIdReq, TransactionDebitResp>)
            .WithName(V1GetTransactDebitById)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapPut("{id:int}", ICRUDEndpoints.Update<UpdateTransactionDebitReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapDelete("{id:int}", ICRUDEndpoints.Delete<DeleteTransactionDebitReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapHead("/", ICommonPagingInfoEndpoints.GetPagingMetaInfo<GetTransactionDebitPagingMetaInfoReq>)
            .RequireCors(ExposedHeadersPagingMetaInfoPolicy);
    }
}
