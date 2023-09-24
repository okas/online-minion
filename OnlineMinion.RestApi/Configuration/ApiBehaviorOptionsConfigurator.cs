using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace OnlineMinion.RestApi.Configuration;

public class ApiBehaviorOptionsConfigurator : IConfigureOptions<ApiBehaviorOptions>
{
    public void Configure(ApiBehaviorOptions options)
    {
        options.SuppressModelStateInvalidFilter = true;
    }
}
