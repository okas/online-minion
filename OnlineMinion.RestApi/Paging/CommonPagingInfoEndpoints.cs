using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using OnlineMinion.Contracts.Shared.Requests;
using static OnlineMinion.RestApi.ProblemHandling.ApiProblemsHandler;

namespace OnlineMinion.RestApi.Paging;

public static class CommonPagingInfoEndpoints
{
    /// <summary>Generic endpoint to query paging metainfo for resource, identified by generic request type.</summary>
    /// <param name="pageSize">It can be provided by mapper explicitly, but default is to look into query string.</param>
    /// <param name="provider"></param>
    /// <param name="sender"></param>
    /// <param name="httpResponse"></param>
    /// <param name="ct"></param>
    public static async Task<Results<NoContent, ProblemHttpResult>> PagingMetaInfo<TRequest>(
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
