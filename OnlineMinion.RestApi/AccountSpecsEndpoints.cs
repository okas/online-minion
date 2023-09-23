using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using OnlineMinion.Common.Utilities;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Shared.Responses;
using OnlineMinion.RestApi.Helpers;
using OnlineMinion.RestApi.Paging;
using static Microsoft.AspNetCore.Http.TypedResults;
using static OnlineMinion.RestApi.Shared.CommonValidationEndpoints;
using static OnlineMinion.RestApi.Init.ApiCorsOptionsConfigurator;
using static OnlineMinion.RestApi.ProblemHandling.ApiProblemsHandler;
using static OnlineMinion.RestApi.Shared.NamedRoutes;

namespace OnlineMinion.RestApi;

// TODO: As soon as we have logging behavior, we can remove logger and make class static.
public class AccountSpecsEndpoints
{
    public static void MapAll(IEndpointRouteBuilder app)
    {
        var apiV1 = app.NewVersionedApi("Account Specs")
            .MapGroup("api/account-specs")
            .HasApiVersion(1)
            .MapToApiVersion(1);

        apiV1.MapPost("", Create);

        apiV1.MapGet("", GetSomePaged)
            .RequireCors(ExposedHeadersPagingMetaInfoPolicy)
            // .WithOpenApi(
            //     o =>
            //     {
            //         o.Summary = Str.GetSomePagedSummary;
            //         o.Description = Str.GetSomePagedDescription;
            //         return o.SetPagingMetaInfoHeaders(200);
            //     }
            // )
            ;

        apiV1.MapGet("{id:int}", GetById)
            .WithName(V1GetAccountSpecById);

        apiV1.MapPut("{id:int}", Update);

        apiV1.MapDelete("{id:int}", Delete);

        apiV1.MapHead("", CommonPagingInfoEndpoints.GetPagingMetaInfo<GetAccountSpecPagingMetaInfoReq>)
            .RequireCors(ExposedHeadersPagingMetaInfoPolicy)
            // .WithOpenApi(
            //     o =>
            //     {
            //         o.Summary = "To probe some paging related data about this resource";
            //         return o.SetPagingMetaInfoHeaders(StatusCodes.Status204NoContent);
            //     }
            // )
            ;

        apiV1.MapGet("descriptors", GetAsDescriptors);

        apiV1.MapHead($"{NewNameValidationRoute}", CheckUniqueNew<CheckAccountSpecUniqueNewReq>);

        apiV1.MapHead($"{ExistingNameValidationRoute}", CheckUniqueExisting<CheckAccountSpecUniqueExistingReq>);
    }

    public static async Task<Results<CreatedAtRoute<ModelIdResp>, ProblemHttpResult>> Create(
        CreateAccountSpecReq rq,
        ISender              sender,
        CancellationToken    ct
    )
    {
        // TODO: param validation & BadRequest return
        var result = await sender.Send(rq, ct);

        return result.MatchFirst<Results<CreatedAtRoute<ModelIdResp>, ProblemHttpResult>>(
            idResp => CreatedAtRoute(idResp, V1GetAccountSpecById, new { idResp.Id, }),
            firstError => CreateApiProblemResult(firstError)
        );
    }

    public static async Task<Results<Ok<IAsyncEnumerable<AccountSpecResp>>, ProblemHttpResult>>
        GetSomePaged(
            [AsParameters] GetSomeModelsPagedReq<AccountSpecResp> rq,
            ISender                                               sender,
            HttpResponse                                          httpResponse,
            [FromServices] ILogger<AccountSpecsEndpoints>         logger, //TODO: add logger category info!
            CancellationToken                                     ct
        )
    {
        // TODO: param validation & BadRequest return
        // if (!ModelState.IsValid)
        // {
        //     return BadRequest(ModelState);
        // }

        var result = await sender.Send(rq, ct);

        return result.MatchFirst<Results<Ok<IAsyncEnumerable<AccountSpecResp>>, ProblemHttpResult>>(
            envelope =>
            {
                httpResponse.SetPagingHeaders(envelope.Paging);
                return Ok(envelope.StreamResult.ToDelayedAsyncEnumerable(20, ct));
            },
            firstError =>
            {
                // TODO: put into logging pipeline (needs creation)
                logger.LogError("Error while getting paged models: {Error}", firstError);
                return CreateApiProblemResult(firstError);
            }
        );
    }

    public static async Task<Results<Ok<AccountSpecResp>, NotFound, ProblemHttpResult>> GetById(
        [AsParameters] GetAccountSpecByIdReq rq,
        ISender                              sender,
        LinkGenerator                        linkGen,
        CancellationToken                    ct
    )
    {
        var result = await sender.Send(rq, ct);

        return result.MatchFirst<Results<Ok<AccountSpecResp>, NotFound, ProblemHttpResult>>(
            model => Ok(model),
            firstError => firstError.Type switch
            {
                ErrorType.NotFound => NotFound(),
                _ => CreateApiProblemResult(firstError, GetResourcePath(linkGen, rq.Id)),
            }
        );
    }

    public static async Task<Results<NoContent, ValidationProblem, ProblemHttpResult>> Update(
        int                  id,
        UpdateAccountSpecReq rq,
        ISender              sender,
        LinkGenerator        linkGen,
        CancellationToken    ct
    )
    {
        if (rq.CheckId(id) is { } validationProblem)
        {
            return validationProblem;
        }

        var result = await sender.Send(rq, ct);

        return result.MatchFirst<Results<NoContent, ValidationProblem, ProblemHttpResult>>(
            _ => NoContent(),
            firstError => CreateApiProblemResult(firstError, GetResourcePath(linkGen, rq.Id))
        );
    }

    public static async Task<Results<NoContent, ProblemHttpResult>> Delete(
        [AsParameters] DeleteAccountSpecReq rq,
        ISender                             sender,
        LinkGenerator                       linkGen,
        CancellationToken                   ct
    )
    {
        var result = await sender.Send(rq, ct);

        return result.MatchFirst<Results<NoContent, ProblemHttpResult>>(
            _ => NoContent(),
            firstError => CreateApiProblemResult(firstError, GetResourcePath(linkGen, rq.Id))
        );
    }

    public static async Task<Results<Ok<IAsyncEnumerable<AccountSpecDescriptorResp>>, ProblemHttpResult>>
        GetAsDescriptors(ISender sender, CancellationToken ct)
    {
        var rq = new GetSomeModelDescriptorsReq<AccountSpecDescriptorResp>();
        var result = await sender.Send(rq, ct);

        return result.MatchFirst<Results<Ok<IAsyncEnumerable<AccountSpecDescriptorResp>>, ProblemHttpResult>>(
            models => Ok(models),
            firstError => CreateApiProblemResult(firstError)
        );
    }

    private static string? GetResourcePath(LinkGenerator linkGen, int id) =>
        linkGen.GetPathByName(V1GetAccountSpecById, new { id, });

    private static class Str
    {
        public const string GetSomePagedDescription =
            "For testing: server sends items in 20ms intervals. This allows to see the effect of Browser response streaming.";

        public const string GetSomePagedSummary = "Get some paged account specs, with paging meta info in header.";
    }
}
