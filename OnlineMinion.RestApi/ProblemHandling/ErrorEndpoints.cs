using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace OnlineMinion.RestApi.ProblemHandling;

public static class ErrorEndpoints
{
    public static void MapDevEndpoints(WebApplication app)
    {
        var api = app.NewVersionedApi("Error Tests")
            .MapGroup("api/error-tests")
            .IsApiVersionNeutral();

        api.MapGet("{throw}", ThrowConditionally);
    }

    /// <summary>This is a test endpoint that throws an unhandled exception.</summary>
    private static NoContent ThrowConditionally(bool @throw = false) =>
        @throw ? throw new("Test exception.") : TypedResults.NoContent();
}
