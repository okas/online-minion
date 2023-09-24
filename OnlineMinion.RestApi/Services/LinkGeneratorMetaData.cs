using Microsoft.AspNetCore.Http;

namespace OnlineMinion.RestApi.Services;

/// <summary>Metadata for <see cref="ResourceLinkGenerator" />.</summary>
/// <remarks>
///     It must be set to Endpoint's <see cref="Endpoint.Metadata" /> collection during endpoints
///     definition using
///     <see cref="Microsoft.AspNetCore.Builder.RoutingEndpointConventionBuilderExtensions.WithMetadata{TBuilder}" />.
/// </remarks>
/// <param name="ResourceRouteName">
///     Route name of the resource, that will be used to generate resource path. This name must be
///     defined <see cref="Microsoft.AspNetCore.Builder.IEndpointConventionBuilder" />, using extension method
///     <see cref="Microsoft.AspNetCore.Builder.RoutingEndpointConventionBuilderExtensions.WithName{TBuilder}" />.
/// </param>
public record LinkGeneratorMetaData(string ResourceRouteName);
