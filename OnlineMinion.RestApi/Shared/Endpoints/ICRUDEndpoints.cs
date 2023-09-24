using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Shared.Responses;
using OnlineMinion.RestApi.Helpers;
using OnlineMinion.RestApi.Paging;
using OnlineMinion.RestApi.Services;
using static Microsoft.AspNetCore.Http.TypedResults;
using static OnlineMinion.RestApi.ProblemHandling.ApiProblemsHandler;

namespace OnlineMinion.RestApi.Shared.Endpoints;

internal interface ICRUDEndpoints
{
    /// <remarks>Needs metadata registration for <see cref="LinkGeneratorMetaData" />!</remarks>
    public static async ValueTask<Results<CreatedAtRoute<ModelIdResp>, ProblemHttpResult>> Create<TRequest>(
        TRequest              rq,
        ISender               sender,
        ResourceLinkGenerator linkGen,
        CancellationToken     ct
    )
        where TRequest : ICreateCommand
    {
        // TODO: param validation & BadRequest return
        var result = await sender.Send(rq, ct);

        return result.MatchFirst<Results<CreatedAtRoute<ModelIdResp>, ProblemHttpResult>>(
            idResp => CreatedAtRoute(idResp, linkGen.ResourceRouteName, new { idResp.Id, }),
            firstError => CreateApiProblemResult(firstError)
        );
    }

    public static async ValueTask<Results<Ok<IAsyncEnumerable<TResponse>>, ProblemHttpResult>> GetSomePaged<TResponse>(
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
                return Ok(envelope.StreamResult);
            },
            firstError =>
            {
                // TODO: put into logging pipeline (needs creation)
                // logger.LogError("Error while getting paged models: {Error}", firstError);
                return CreateApiProblemResult(firstError);
            }
        );
    }

    /// <remarks>Needs metadata registration for <see cref="LinkGeneratorMetaData" />!</remarks>
    public static async ValueTask<Results<Ok<TResponse>, NotFound, ProblemHttpResult>> GetById<TRequest, TResponse>(
        [AsParameters] TRequest rq,
        ISender                 sender,
        ResourceLinkGenerator   linkGen,
        CancellationToken       ct
    )
        where TRequest : IGetByIdRequest<TResponse>
    {
        var result = await sender.Send(rq, ct);

        return result.MatchFirst<Results<Ok<TResponse>, NotFound, ProblemHttpResult>>(
            model => Ok(model),
            firstError => firstError.Type switch
            {
                ErrorType.NotFound => NotFound(),
                _ => CreateApiProblemResult(firstError, linkGen.GetResourceUri(rq.Id)),
            }
        );
    }

    /// <remarks>Needs metadata registration for <see cref="LinkGeneratorMetaData" />!</remarks>
    public static async ValueTask<Results<NoContent, ValidationProblem, ProblemHttpResult>> Update<TRequest>(
        int                   id,
        TRequest              rq,
        ISender               sender,
        ResourceLinkGenerator linkGen,
        CancellationToken     ct
    )
        where TRequest : IUpdateCommand
    {
        if (rq.CheckId(id) is { } validationProblem)
        {
            return validationProblem;
        }

        var result = await sender.Send(rq, ct);

        return result.MatchFirst<Results<NoContent, ValidationProblem, ProblemHttpResult>>(
            _ => NoContent(),
            firstError => CreateApiProblemResult(firstError, linkGen.GetResourceUri(rq.Id))
        );
    }

    /// <remarks>Needs metadata registration for <see cref="LinkGeneratorMetaData" />!</remarks>
    public static async ValueTask<Results<NoContent, ProblemHttpResult>> Delete<TRequest>(
        [AsParameters] TRequest rq,
        ISender                 sender,
        ResourceLinkGenerator   linkGen,
        CancellationToken       ct
    )
        where TRequest : IDeleteByIdCommand
    {
        var result = await sender.Send(rq, ct);

        return result.MatchFirst<Results<NoContent, ProblemHttpResult>>(
            _ => NoContent(),
            firstError => CreateApiProblemResult(firstError, linkGen.GetResourceUri(rq.Id))
        );
    }

    /// <summary>
    ///     NB! This method do not use paging! It is intended to return resources as  options for select lists
    ///     or other descriptive needs in UI, where full resource data is not needed.<br />
    ///     It won't set paging headers, contrary to <see cref="GetSomePaged{TResponse}" />.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ct"></param>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    public static async ValueTask<Results<Ok<IAsyncEnumerable<TResponse>>, ProblemHttpResult>>
        GetAsDescriptors<TResponse>(ISender sender, CancellationToken ct)
        where TResponse : IHasIntId
    {
        var rq = new GetSomeModelDescriptorsReq<TResponse>();
        var result = await sender.Send(rq, ct);

        return result.MatchFirst<Results<Ok<IAsyncEnumerable<TResponse>>, ProblemHttpResult>>(
            models => Ok(models),
            error => CreateApiProblemResult(error)
        );
    }
}