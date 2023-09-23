using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using OnlineMinion.Contracts;

namespace OnlineMinion.RestApi.Helpers;

// TODO: endpoint filter candidate?
public static class RequestHelpers
{
    /// <summary>
    ///     Checks id equality between route parameter and request body.<br />
    ///     If they are not equal returns `null`, otherwise  <see cref="ValidationProblem" /> with error details.
    /// </summary>
    /// <param name="req">Request body as <see cref="IHasIntId" /></param>
    /// <param name="id">Value from route parameter.</param>
    /// <returns></returns>
    public static ValidationProblem? CheckId(this IHasIntId req, int id)
    {
        if (id == req.Id)
        {
            return null;
        }

        var errors = new Dictionary<string, string[]>
        {
            { "id", new[] { "Id in route and in body are not the same.", } },
        };

        return TypedResults.ValidationProblem(errors);
    }
}
