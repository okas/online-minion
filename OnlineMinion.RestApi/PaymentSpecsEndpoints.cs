using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using OnlineMinion.Common.Utilities;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Contracts.PaymentSpec.Responses;
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

public class PaymentSpecsEndpoints
{
    public static void MapAll(IEndpointRouteBuilder app)
    {
        var apiV1 = app.NewVersionedApi("Payment Specs")
            .MapGroup("api/payment-specs")
            .HasApiVersion(1)
            .MapToApiVersion(1);

        apiV1.MapPost("", Create);

        apiV1.MapGet("", GetSomePaged)
            .RequireCors(ExposedHeadersPagingMetaInfoPolicy);

        apiV1.MapGet("{id:int}", GetById)
            .WithName(V1GetPaymentSpecById);

        apiV1.MapPut("{id:int}", Update);

        apiV1.MapDelete("{id:int}", Delete);

        apiV1.MapHead("", CommonPagingInfoEndpoints.GetPagingMetaInfo<GetPaymentSpecPagingMetaInfoReq>)
            .RequireCors(ExposedHeadersPagingMetaInfoPolicy);

        apiV1.MapGet("descriptors", GetAsDescriptors);

        apiV1.MapHead($"{NewNameValidationRoute}", CheckUniqueNew<CheckPaymentSpecUniqueNewReq>);

        apiV1.MapHead($"{ExistingNameValidationRoute}", CheckUniqueExisting<CheckPaymentSpecUniqueExistingReq>);
    }

    public static async ValueTask<Results<CreatedAtRoute<ModelIdResp>, ProblemHttpResult>> Create(
        CreatePaymentSpecReq rq,
        ISender              sender,
        CancellationToken    ct
    )
    {
        // TODO: param validation & BadRequest return
        var result = await sender.Send(rq, ct);

        return result.MatchFirst<Results<CreatedAtRoute<ModelIdResp>, ProblemHttpResult>>(
            idResp => CreatedAtRoute(idResp, V1GetPaymentSpecById, new { idResp.Id, }),
            firstError => CreateApiProblemResult(firstError)
        );
    }

    public static async Task<Results<Ok<IAsyncEnumerable<PaymentSpecResp>>, ProblemHttpResult>>
        GetSomePaged(
            [AsParameters] GetSomeModelsPagedReq<PaymentSpecResp> rq,
            ISender                                               sender,
            HttpResponse                                          httpResponse,
            [FromServices] ILogger<PaymentSpecsEndpoints>         logger, //TODO: add logger category info!
            CancellationToken                                     ct
        )
    {
        // TODO: param validation & BadRequest return
        // if (!ModelState.IsValid)
        // {
        //     return BadRequest(ModelState);
        // }

        var result = await sender.Send(rq, ct);

        return result.MatchFirst<Results<Ok<IAsyncEnumerable<PaymentSpecResp>>, ProblemHttpResult>>(
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

    public static async Task<Results<Ok<PaymentSpecResp>, NotFound, ProblemHttpResult>> GetById(
        [AsParameters] GetPaymentSpecByIdReq rq,
        ISender                              sender,
        LinkGenerator                        linkGen,
        CancellationToken                    ct
    )
    {
        var result = await sender.Send(rq, ct);

        return result.MatchFirst<Results<Ok<PaymentSpecResp>, NotFound, ProblemHttpResult>>(
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
        UpdatePaymentSpecReq rq,
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
        [AsParameters] DeletePaymentSpecReq rq,
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

    public static async Task<Results<Ok<IAsyncEnumerable<PaymentSpecDescriptorResp>>, ProblemHttpResult>>
        GetAsDescriptors(ISender sender, CancellationToken ct)
    {
        var rq = new GetSomeModelDescriptorsReq<PaymentSpecDescriptorResp>();
        var result = await sender.Send(rq, ct);

        return result.MatchFirst<Results<Ok<IAsyncEnumerable<PaymentSpecDescriptorResp>>, ProblemHttpResult>>(
            models => Ok(models),
            firstError => CreateApiProblemResult(firstError)
        );
    }

    private static string? GetResourcePath(LinkGenerator linkGen, int id) =>
        linkGen.GetPathByName(V1GetPaymentSpecById, new { id, });
}
