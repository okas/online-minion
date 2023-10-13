using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using OnlineMinion.RestApi.ProblemHandling;

namespace OnlineMinion.RestApi;

public interface ICommonValidationEndpoints
{
    public const string NewNameValidationRoute = "validate-available-name/{memberValue:required}";
    public const string NewIBANValidationRoute = "validate-available-iban/{memberValue:required}";

    public const string ExistingNameValidationRoute = $"{NewNameValidationRoute}/except-id/{{ownid:guid}}";
    public const string ExistingIBANValidationRoute = $"{NewIBANValidationRoute}/except-id/{{ownid:guid}}";

    //TODO: copy docs from other endpoints

    public static async Task<Results<NoContent, ProblemHttpResult>> CheckUniqueByMember<TRequest>(
        [AsParameters] TRequest rq,
        ISender                 sender,
        CancellationToken       ct
    )
        where TRequest : IRequest<ErrorOr<Success>>
    {
        var result = await sender.Send(rq, ct).ConfigureAwait(false);

        return result.MatchFirst<Results<NoContent, ProblemHttpResult>>(
            _ => TypedResults.NoContent(),
            firstError => ApiProblemResults.CreateApiProblemResult(firstError)
        );
    }
}
