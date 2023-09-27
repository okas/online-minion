using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Contracts.Transactions.Credit.Responses;
using OnlineMinion.RestApi.CommonEndpoints;
using OnlineMinion.RestApi.Helpers;
using OnlineMinion.RestApi.Services.LinkGeneration;
using static OnlineMinion.RestApi.Configuration.ApiCorsOptionsConfigurator;

namespace OnlineMinion.RestApi;

public class TransactionCreditsEndpoints : ICommonCrudEndpoints, ICommonPagingInfoEndpoints
{
    public const string V1GetTransactCreditById = nameof(V1GetTransactCreditById);

    public static void MapAll(IEndpointRouteBuilder app)
    {
        var apiV1 = app.NewVersionedApi("Credit Transactions")
            .MapGroup("api/transactions/credits")
            .HasApiVersion(1)
            .MapToApiVersion(1);

        var linkGeneratorMetaData = new ResourceLinkGeneratorMetaData(V1GetTransactCreditById);

        apiV1.MapPost("/", ICommonCrudEndpoints.Create<CreateTransactionCreditReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapGet("/", ICommonCrudEndpoints.GetSomePaged<TransactionCreditResp>)
            .RequireCors(ExposedHeadersPagingMetaInfoPolicy);

        apiV1.MapGet("{id:guid}", ICommonCrudEndpoints.GetById<GetTransactionCreditByIdReq, TransactionCreditResp>)
            .WithName(V1GetTransactCreditById)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapPut("{id:guid}", ICommonCrudEndpoints.Update<UpdateTransactionCreditReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapDelete("{id:guid}", ICommonCrudEndpoints.Delete<DeleteTransactionCreditReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapHead("/", ICommonPagingInfoEndpoints.GetPagingMetaInfo<GetTransactionCreditPagingMetaInfoReq>)
            .RequireCors(ExposedHeadersPagingMetaInfoPolicy);
    }
}
