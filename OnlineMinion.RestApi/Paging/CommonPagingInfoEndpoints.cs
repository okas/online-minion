using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.RestApi.Init;
using static OnlineMinion.RestApi.ProblemHandling.ApiProblemsHandler;

namespace OnlineMinion.RestApi.Paging;

public static class CommonPagingInfoEndpoints
{
    /// <summary>Generic endpoint to query paging metainfo for resource, identified by generic request type.</summary>
    /// <param name="rq">Using page size, query pagination related info for given resource or model.</param>
    /// <param name="sender"></param>
    /// <param name="httpResponse">
    ///     Query result is returned using Pagination headers, see:
    ///     <see cref="OnlineMinion.Contracts.CustomHeaderNames" />.
    /// </param>
    /// <param name="ct"></param>
    /// <remarks>
    ///     Important: as returned headers are custom, CORS policy should be enabled for this endpoint,
    ///     this can be done using this policy name's constant, see:
    ///     <see cref="ApiCorsOptionsConfigurator.ExposedHeadersPagingMetaInfoPolicy" />.
    /// </remarks>
    public static async Task<Results<NoContent, ProblemHttpResult>> GetPagingMetaInfo<TRequest>(
        [AsParameters] TRequest rq,
        ISender                 sender,
        HttpResponse            httpResponse,
        CancellationToken       ct
    ) where TRequest : IGetPagingInfoRequest
    {
        // var rq = ActivatorUtilities.CreateInstance<TRequest>(provider, pageSize)
        // ?? throw new InvalidOperationException(GetMsgInvalidTypeParameterInit<TRequest>(pageSize));

        var result = await sender.Send(rq, ct);

        return result.MatchFirst<Results<NoContent, ProblemHttpResult>>(
            pagingMetaInfo =>
            {
                httpResponse.SetPagingHeaders(pagingMetaInfo);
                return TypedResults.NoContent();
            },
            firstError => CreateApiProblemResult(firstError)
        );
    }

    private static string GetMsgInvalidTypeParameterInit<TRequest>(int pageSize)
        where TRequest : IGetPagingInfoRequest =>
        $"Could not create instance of {typeof(TRequest).FullName} with page size `{pageSize}`.";

    private static class Str
    {
        public const string Description = "Using page size, get count of total items and count of pages.";
    }
}
