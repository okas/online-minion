using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using OnlineMinion.Common.Utilities;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.RestApi.Helpers;
using OnlineMinion.RestApi.Paging;
using OnlineMinion.RestApi.ProblemHandling;
using OnlineMinion.RestApi.Services;
using OnlineMinion.RestApi.Shared;
using OnlineMinion.RestApi.Shared.Endpoints;
using static OnlineMinion.RestApi.Shared.ICommonValidationEndpoints;
using static OnlineMinion.RestApi.Configuration.ApiCorsOptionsConfigurator;
using static OnlineMinion.RestApi.Shared.NamedRoutes;

namespace OnlineMinion.RestApi;

public class AccountSpecsEndpoints
    : ICRUDEndpoints, ICommonDescriptorEndpoints, ICommonPagingInfoEndpoints, ICommonValidationEndpoints
{
    public static void MapAll(IEndpointRouteBuilder app)
    {
        var apiV1 = app.NewVersionedApi("Account Specs")
            .MapGroup("api/account-specs")
            .HasApiVersion(1)
            .MapToApiVersion(1);

        var linkGeneratorMetaData = new LinkGeneratorMetaData(V1GetAccountSpecById);

        apiV1.MapPost("/", ICRUDEndpoints.Create<CreateAccountSpecReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapGet("/", GetSomePaged<AccountSpecResp>) // TODO: Custom, throttling response streaming
            .RequireCors(ExposedHeadersPagingMetaInfoPolicy);

        apiV1.MapGet("{id:int}", ICRUDEndpoints.GetById<GetAccountSpecByIdReq, AccountSpecResp>)
            .WithName(V1GetAccountSpecById)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapPut("{id:int}", ICRUDEndpoints.Update<UpdateAccountSpecReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapDelete("{id:int}", ICRUDEndpoints.Delete<DeleteAccountSpecReq>)
            .WithMetadata(linkGeneratorMetaData);

        apiV1.MapHead("/", ICommonPagingInfoEndpoints.GetPagingMetaInfo<GetAccountSpecPagingMetaInfoReq>)
            .RequireCors(ExposedHeadersPagingMetaInfoPolicy);

        apiV1.MapGet("descriptors", ICommonDescriptorEndpoints.GetAsDescriptors<AccountSpecDescriptorResp>);

        apiV1.MapHead($"{NewNameValidationRoute}", CheckUniqueNew<CheckAccountSpecUniqueNewReq>);

        apiV1.MapHead($"{ExistingNameValidationRoute}", CheckUniqueExisting<CheckAccountSpecUniqueExistingReq>);
    }

    /// <summary>
    ///     DEMO endpoint, for testing: server sends items in 20ms intervals. This allows to see the effect of
    ///     Browser response streaming. <br /> <br />
    ///     Gets resources, paged. Response Body will contain only collection of resources, paging headers will be set
    ///     in response headers.
    /// </summary>
    /// <inheritdoc cref="ICRUDEndpoints.GetSomePaged{TResponse}" />
    private static async ValueTask<Results<Ok<IAsyncEnumerable<TResponse>>, ProblemHttpResult>> GetSomePaged<TResponse>(
        [AsParameters] GetSomeModelsPagedReq<TResponse> rq,
        ISender                                         sender,
        HttpResponse                                    httpResponse,
        CancellationToken                               ct
    )
        where TResponse : IHasIntId
    {
        var result = await sender.Send(rq, ct);

        return result.MatchFirst<Results<Ok<IAsyncEnumerable<TResponse>>, ProblemHttpResult>>(
            envelope =>
            {
                httpResponse.SetPagingHeaders(envelope.Paging);
                return TypedResults.Ok(envelope.StreamResult.ToDelayedAsyncEnumerable(20, ct));
            },
            firstError =>
            {
                // TODO: put into logging pipeline (needs creation)
                // logger.LogError("Error while getting paged models: {Error}", firstError);
                return ApiProblemsHandler.CreateApiProblemResult(firstError);
            }
        );
    }

    private static class Str
    {
        public const string GetSomePagedDescription =
            "For testing: server sends items in 20ms intervals. This allows to see the effect of Browser response streaming.";

        public const string GetSomePagedSummary = "Get some paged account specs, with paging meta info in header.";
    }
}
