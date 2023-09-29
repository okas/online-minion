using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using OnlineMinion.Application.Contracts.Shared.Requests;
using OnlineMinion.RestApi.ProblemHandling;

namespace OnlineMinion.RestApi;

public interface ICommonValidationEndpoints
{
    public const string NewNameValidationRoute = "validate-available-name/{memberValue:required:length(2,50)}";

    public const string ExistingNameValidationRoute = $"{NewNameValidationRoute}/except-id/{{ownid:guid}}";

    //TODO: copy docs from other endpoints

    public static async Task<Results<NoContent, ProblemHttpResult>> CheckUniqueNew<TRequest>(
        [AsParameters] TRequest rq,
        ISender                 sender,
        CancellationToken       ct
    ) where TRequest : ICheckUniqueNewModelByMemberRequest
    {
        var result = await sender.Send(rq, ct).ConfigureAwait(false);
        return ConvertToHttpResult(result);
    }

    //TODO: copy docs from other endpoints
    public static async Task<Results<NoContent, ProblemHttpResult>> CheckUniqueExisting<TRequest>(
        [AsParameters] TRequest rq,
        ISender                 sender,
        CancellationToken       ct
    ) where TRequest : ICheckUniqueExistingModelByMemberRequest
    {
        var result = await sender.Send(rq, ct).ConfigureAwait(false);
        return ConvertToHttpResult(result);
    }

    private static Results<NoContent, ProblemHttpResult> ConvertToHttpResult(ErrorOr<Success> result) =>
        result.MatchFirst<Results<NoContent, ProblemHttpResult>>(
            _ => TypedResults.NoContent(),
            firstError => ApiProblemResults.CreateApiProblemResult(firstError)
        );
}
