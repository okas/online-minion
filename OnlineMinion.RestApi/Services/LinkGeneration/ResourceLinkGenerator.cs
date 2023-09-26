using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace OnlineMinion.RestApi.Services.LinkGeneration;

/// <summary>A service that generates a resource path ur Uri for a given resource ID.</summary>
/// <param name="httpContextAccessor">Provides access to <see cref="Endpoint" /> of the current HTTP request.</param>
/// <param name="linkGenerator">LinkGenerator of the current HTTP request.</param>
/// <remarks>
///     NB! Relies on <see cref="IHttpContextAccessor" /> feature, make sure it is registered like in example!<br />
///     It is intended to be injected into Minimal API endpoint or controller.<br />
///     <para>
///         It might need a wrapper creation and registration for HttpContextAccessor:
///         <a href="https://www.code4it.dev/blog/inject-httpcontext/#further-improvements">code4it.dev</a>.
///     </para>
/// </remarks>
/// <example>services.AddHttpContextAccessor();</example>
/// <inheritdoc cref="GetMetadata" />
[UsedImplicitly]
public class ResourceLinkGenerator(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
{
    private const string MsgHttpContextIsNull = "HttpContext is null.";
    private const string MsgEndpointIsNull = "Endpoint is null.";

    private const string MsgNoMetadata = $"{nameof(ResourceLinkGenerator)
    } can only be used in the context of an endpoint with {nameof(ResourceLinkGeneratorMetaData)} metadata.";

    private readonly HttpContext _httpContext = httpContextAccessor.HttpContext!;

    private readonly ResourceLinkGeneratorMetaData _metaData = GetMetadata(httpContextAccessor);

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
        linkGenerator.GetUriByName(_httpContext, ResourceRouteName, GetRouteValues(idRouteParamName, id));

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
        linkGenerator.GetPathByName(_httpContext, ResourceRouteName, GetRouteValues(idRouteParamName, id));

    private static RouteValueDictionary GetRouteValues<TValue>(string key, TValue value) =>
        new() { { GetCleanName(key), value }, };

    /// <summary>
    ///     Takes the last part of provided expression, e.g. "Id" from "modelObj" or "id" from "modelObj.Id".
    /// </summary>
    private static string GetCleanName(string raw) => raw.AsSpan(raw.LastIndexOf('.') + 1).ToString();

    /// <summary>Retrieves <see cref="ArgumentNullException" /> with guards. Ensures that metadata is available or throws.</summary>
    /// <exception cref="InvalidOperationException">
    ///     On initialization, most probably HttpContextAccessor is not registered in DI container,
    ///     see example how to do it.
    /// </exception>
    /// <exception cref="IHttpContextAccessor.HttpContext">
    ///     On initialization, when <see cref="IHttpContextAccessor" /> is null in
    ///     <see cref="InvalidOperationException" /> instance.
    /// </exception>
    /// ///
    /// <exception cref="EndpointHttpContextExtensions.GetEndpoint">
    ///     On initialization, when <see cref="EndpointHttpContextExtensions" /> is null.
    /// </exception>
    /// <exception cref="ResourceLinkGeneratorMetaData">
    ///     On initialization, when the endpoint does not have <see cref="ResourceLinkGeneratorMetaData" /> metadata.
    /// </exception>
    private static ResourceLinkGeneratorMetaData GetMetadata(
        [NotNullIfNotNull(nameof(accessor))] IHttpContextAccessor? accessor,
        [CallerArgumentExpression(nameof(accessor))]
        string argName = null!
    ) =>
        accessor is not null
            ? accessor.HttpContext is { } context
                ? context.GetEndpoint() is { } endpoint
                    ? endpoint.Metadata.GetMetadata<ResourceLinkGeneratorMetaData>()
                      ?? throw new InvalidOperationException(MsgNoMetadata)
                    : throw new InvalidOperationException(MsgEndpointIsNull)
                : throw new InvalidOperationException(MsgHttpContextIsNull)
            : throw new ArgumentNullException(argName);
}
