using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace OnlineMinion.RestApi.Services.LinkGeneration;

/// <summary>A service that generates a resource path ur Uri for a given resource ID.</summary>
/// <remarks>
///     NB! It Relies on <see cref="IHttpContextAccessor" /> feature, make sure it is registered like in example!<br />
///     It is intended to be injected into Minimal API endpoint or controller.<br />
///     <para>
///         It might need a wrapper creation and registration for HttpContextAccessor:
///         <a href="https://www.code4it.dev/blog/inject-httpcontext/#further-improvements">code4it.dev</a>.
///     </para>
/// </remarks>
/// <example>
///     <b>Register HttpAccessor feature:</b>
///     <code>services.AddHttpContextAccessor();</code>
///     <br />
///     <b>Provide metadata for the endpoint:</b>
///     <code>
///          app.Map{HttpMethod}(...)
///             .WithMetadata(new ResourceLinkGeneratorMetaData("ResourceRouteName"));
/// </code>
/// </example>
[UsedImplicitly]
public class ResourceLinkGenerator
{
    private const string MsgNoMetadata = $"{nameof(ResourceLinkGenerator)
    } can only be used in the context of an endpoint invokation with {nameof(ResourceLinkGeneratorMetaData)} metadata.";

    private readonly HttpContext _httpContext;
    private readonly LinkGenerator _linkGenerator;
    private readonly ResourceLinkGeneratorMetaData _metaData;

    /// <inheritdoc cref="ResourceLinkGenerator" />
    /// <param name="httpContextAccessor">Provides access to <see cref="Endpoint" /> of the current HTTP request.</param>
    /// <param name="linkGenerator">LinkGenerator of the current HTTP request.</param>
    /// <exception cref="ArgumentNullException">When <see cref="linkGenerator" /> is null.</exception>
    /// <exception cref="ArgumentNullException">
    ///     When <see cref="httpContextAccessor" /> <i>or</i> its prop <see cref="IHttpContextAccessor.HttpContext" /> is null,
    ///     most probably HttpContextAccessor is not registered in DI container, see example how to do it.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     When <c>httpContextAccessor.HttpContext.</c><see cref="EndpointHttpContextExtensions.GetEndpoint" /> is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     When <see cref="Endpoint" /> does not have <see cref="ResourceLinkGeneratorMetaData" /> metadata set.<br />
    ///     If endpoint is set, check if it has <see cref="ResourceLinkGeneratorMetaData" /> metadata set. See examples,
    ///     how to do it.
    /// </exception>
    public ResourceLinkGenerator(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
    {
        Endpoint? endpoint;

        ArgumentNullException.ThrowIfNull(linkGenerator);
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        ArgumentNullException.ThrowIfNull(httpContextAccessor.HttpContext);
        ArgumentNullException.ThrowIfNull(endpoint = httpContextAccessor.HttpContext.GetEndpoint());

        _httpContext = httpContextAccessor.HttpContext!;
        _linkGenerator = linkGenerator;

        _metaData = endpoint.Metadata.GetMetadata<ResourceLinkGeneratorMetaData>()
                    ?? throw new InvalidOperationException(MsgNoMetadata);
    }

    /// <summary>Route name of the resource, that is provided via Endpoint metadata.</summary>
    public string ResourceRouteName => _metaData.ResourceRouteName;

    /// <summary>
    ///     Generates a resource absolute Uri for a given resource id.
    /// </summary>
    /// <inheritdoc cref="GetResourcePath{TValue}" />
    public string? GetResourceUri<TValue>(
        [DisallowNull]                         TValue id,
        [CallerArgumentExpression(nameof(id))] string idRouteParamName = ""
    ) =>
        _linkGenerator.GetUriByName(_httpContext, ResourceRouteName, GetRouteValues(idRouteParamName, id));

    /// <summary>
    ///     Generates a resource absolute path for a given resource id.
    /// </summary>
    /// <param name="id">ID value for route parameter.</param>
    /// <param name="idRouteParamName">
    ///     Optional. Name for the "ID" route parameter. Defaults to the name of the argument for <paramref name="id" />
    ///     parameter in calling code. In case of string value as an argument, "<paramref name="id" />" is used.
    ///     See: <see cref="CallerArgumentExpressionAttribute" />.
    /// </param>
    /// <typeparam name="TValue">Type of "ID" value.</typeparam>
    /// <returns>Generated string or <see langword="null" />, if it cannot be done.</returns>
    public string? GetResourcePath<TValue>(
        [DisallowNull]                         TValue id,
        [CallerArgumentExpression(nameof(id))] string idRouteParamName = ""
    ) =>
        _linkGenerator.GetPathByName(_httpContext, ResourceRouteName, GetRouteValues(idRouteParamName, id));

    private static RouteValueDictionary GetRouteValues<TValue>(string key, TValue value) =>
        new() { { GetCleanName(key), value }, };

    /// <summary>
    ///     Takes the last part of provided expression, e.g. "Id" from "modelObj" or "id" from "modelObj.Id".
    /// </summary>
    private static string GetCleanName(string raw) => raw.AsSpan(raw.LastIndexOf('.') + 1).ToString();
}
