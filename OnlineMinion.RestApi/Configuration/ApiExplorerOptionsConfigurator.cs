using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;

namespace OnlineMinion.RestApi.Configuration;

public class ApiExplorerOptionsConfigurator : IConfigureOptions<ApiExplorerOptions>
{
    public void Configure(ApiExplorerOptions options)
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = false;
    }
}
