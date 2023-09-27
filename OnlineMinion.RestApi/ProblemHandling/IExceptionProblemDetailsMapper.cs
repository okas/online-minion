using Microsoft.AspNetCore.Http;

namespace OnlineMinion.RestApi.ProblemHandling;

public interface IExceptionProblemDetailsMapper
{
    void MapExceptions(ProblemDetailsContext context);
}
