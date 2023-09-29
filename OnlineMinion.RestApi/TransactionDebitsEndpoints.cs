using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using OnlineMinion.Contracts.Transactions.Debit.Requests;
using OnlineMinion.Contracts.Transactions.Debit.Responses;
using OnlineMinion.RestApi.Helpers;
using OnlineMinion.RestApi.Services.LinkGeneration;
using static OnlineMinion.RestApi.Configuration.ApiCorsOptionsConfigurator;

namespace OnlineMinion.RestApi;

public class TransactionDebitsEndpoints : ICommonCrudEndpoints, ICommonPagingInfoEndpoints
{
    public const string V1GetTransactDebitById = nameof(V1GetTransactDebitById);

    public static void MapAll(IEndpointRouteBuilder app)
    {
        var apiV1 = app.NewVersionedApi("Debit Transactions")
            .MapGroup("api/transactions/debits")
            .HasApiVersion(1)
            .MapToApiVersion(1);

        var linkGeneratorMetaData = new ResourceLinkGeneratorMetaData(V1GetTransactDebitById);

        apiV1.MapPost("/", ICommonCrudEndpoints.Create<CreateTransactionDebitReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapGet("/", ICommonCrudEndpoints.GetSomePaged<TransactionDebitResp>)
            .RequireCors(ExposedHeadersPagingMetaInfoPolicy);

        apiV1.MapGet("{id:guid}", ICommonCrudEndpoints.GetById<GetTransactionDebitByIdReq, TransactionDebitResp>)
            .WithName(V1GetTransactDebitById)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapPut("{id:guid}", ICommonCrudEndpoints.Update<UpdateTransactionDebitReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapDelete("{id:guid}", ICommonCrudEndpoints.Delete<DeleteTransactionDebitReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapHead("/", ICommonPagingInfoEndpoints.GetPagingMetaInfo<GetTransactionDebitPagingMetaInfoReq>)
            .RequireCors(ExposedHeadersPagingMetaInfoPolicy);
    }
}
