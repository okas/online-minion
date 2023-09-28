using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace OnlineMinion.Common;

/// <summary>
///     Wrapped<see cref="Mediator" /> class to be able to use it as <see cref="IAsyncValidatorSender" />.
/// </summary>
/// <remarks>
///     NB! This class must be registered to the DI container only after MediatR's normal registration using
///     <see
///         cref="ServiceCollectionExtensions.AddMediatR(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Action{Microsoft.Extensions.DependencyInjection.MediatRServiceConfiguration})" />
///     has been done.<br />
///     Reason is that this extension method set up MediatR's configuration and it's registration must be done before.
/// </remarks>
[UsedImplicitly]
public class MediatorWrapper : Mediator, IAsyncValidatorSender
{
    public MediatorWrapper(IServiceProvider serviceProvider) : base(serviceProvider) { }

    public MediatorWrapper(IServiceProvider serviceProvider, INotificationPublisher publisher)
        : base(serviceProvider, publisher) { }
}
