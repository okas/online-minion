using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.RestApi.ProblemHandling;

namespace OnlineMinion.RestApi;

public interface ICommonDescriptorEndpoints
{
    public const string DescriptorsCommonRoute = "descriptors";

    /// <summary>
    ///     NB! This method do not use paging! It is intended to return resources as  options for select lists
    ///     or other descriptive needs in UI, where full resource data is not needed.<br />
    ///     It won't set paging headers, contrary to
    ///     <see cref="ICommonCrudEndpoints.GetSomePaged{TResponse}">
    ///         ICommonCrudEndpoints.GetSomePaged&lt;TResponse&gt;
    ///     </see>
    ///     endpoint method.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ct"></param>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    public static async ValueTask<Results<Ok<IAsyncEnumerable<TResponse>>, ProblemHttpResult>>
        GetAsDescriptors<TResponse>(ISender sender, CancellationToken ct)
        where TResponse : IHasId
    {
        var rq = new GetSomeModelDescriptorsReq<TResponse>();
        var result = await sender.Send(rq, ct);

        return result.MatchFirst<Results<Ok<IAsyncEnumerable<TResponse>>, ProblemHttpResult>>(
            models => TypedResults.Ok(models),
            error => ApiProblemResults.CreateApiProblemResult(error)
        );
    }
}
