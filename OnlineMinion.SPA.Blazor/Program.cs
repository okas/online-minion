using FluentValidation;
using IL.FluentValidation.Extensions.Options;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OnlineMinion.Application.RequestValidation;
using OnlineMinion.RestApi.Client.HttpRequestMessageHandlers;
using OnlineMinion.SPA.Blazor;
using OnlineMinion.SPA.Blazor.Configuration;
using OnlineMinion.SPA.Blazor.Helpers;
using OnlineMinion.SPA.Blazor.Settings;
using Radzen;

var hostBuilder = WebAssemblyHostBuilder.CreateDefault(args);

#region Container setup

var services = hostBuilder.Services;

hostBuilder.Logging.AddConfiguration(
    hostBuilder.Configuration.GetSection("Logging")
);

services.AddLogging();

services.AddHttpClient(
    Constants.HostClient,
    client => client.BaseAddress = new(hostBuilder.HostEnvironment.BaseAddress)
);

services.AddOptions<WebAppSettings>()
    .BindConfiguration(nameof(WebAppSettings))
    .ValidateWithFluentValidator()
    .ValidateOnStart();

services.AddTransient<WasmHttpRequestMessageConfiguration>()
    .AddRestApiClient()
    .AddHttpMessageHandler(
        sp => new DelegatedRequestHandler(
            sp.GetRequiredService<WasmHttpRequestMessageConfiguration>().EnableBrowserResponseStreamingForGet
        )
    );

services.AddTransient<IAsyncValidatorSender, MediatorDecorator>();

services.AddSingleton<StateContainer>();

services.AddApplicationRequestValidation();
// For settings validation.
services.AddValidatorsFromAssemblyContaining<Program>();

services.AddScoped<DialogService>();

#endregion

#region Components setup

hostBuilder.RootComponents.Add<App>("#app");
hostBuilder.RootComponents.Add<HeadOutlet>("head::after");

#endregion

var webAssemblyHost = hostBuilder.Build();

await webAssemblyHost.RunAsync().ConfigureAwait(false);
