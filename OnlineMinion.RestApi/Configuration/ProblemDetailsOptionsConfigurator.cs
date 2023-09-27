using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OnlineMinion.RestApi.ProblemHandling;

namespace OnlineMinion.RestApi.Configuration;

[UsedImplicitly]
public class ProblemDetailsOptionsConfigurator(IExceptionProblemDetailsMapper mapper)
    : IConfigureOptions<ProblemDetailsOptions>
{
    public void Configure(ProblemDetailsOptions options) => options.CustomizeProblemDetails = Customize;

    private void Customize(ProblemDetailsContext context)
    {
        mapper.MapExceptions(context);

        context.ProblemDetails.Extensions["TraceId"] = context.HttpContext.TraceIdentifier;
    }
}
