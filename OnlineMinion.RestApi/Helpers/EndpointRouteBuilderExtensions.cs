using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace OnlineMinion.RestApi.Helpers;

public static class EndpointRouteBuilderExtensions
{
    private const string MapEndpointUnreferencedCodeWarning =
        "This API may perform reflection on the supplied delegate and its parameters. These types may be trimmed if not directly referenced.";

    private const string MapEndpointDynamicCodeWarning =
        "This API may perform reflection on the supplied delegate and its parameters. These types may require generated code and aren't compatible with native AOT applications.";

    /// <summary>
    ///     Adds a <see cref="RouteEndpoint" /> to the <see cref="IEndpointRouteBuilder" /> that matches HTTP HEAD requests
    ///     for the specified pattern.
    /// </summary>
    /// <param name="endpoints">The <see cref="IEndpointRouteBuilder" /> to add the route to.</param>
    /// <param name="pattern">The route pattern.</param>
    /// <param name="handler">The delegate executed when the endpoint is matched.</param>
    /// <returns>A <see cref="RouteHandlerBuilder" /> that can be used to further customize the endpoint.</returns>
    [AspMinimalApiDeclaration(HttpVerb = "HEAD")]
    [RequiresUnreferencedCode(MapEndpointUnreferencedCodeWarning)]
    [RequiresDynamicCode(MapEndpointDynamicCodeWarning)]
    public static RouteHandlerBuilder MapHead(
        this                                   IEndpointRouteBuilder endpoints,
        [StringSyntax("Route")][RouteTemplate] string                pattern,
        [AspMinimalApiHandler]                 Delegate              handler
    ) =>
        endpoints.MapMethods(pattern, new[] { HttpMethods.Head, }, handler);
}
