using FluentValidation;
using IL.FluentValidation.Extensions.Options;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OnlineMinion.Common.Shared.Validation;
using OnlineMinion.RestApi.Client.HttpRequestMessageHandlers;
using OnlineMinion.Web;
using OnlineMinion.Web.Configuration;
using OnlineMinion.Web.Settings;
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

services.AddSingleton<StateContainer>();

services.AddValidatorsFromAssemblyContaining<HasIntIdValidator>();
services.AddValidatorsFromAssemblyContaining<Program>();

services.AddScoped<DialogService>();

#endregion

#region Components setup

hostBuilder.RootComponents.Add<App>("#app");
hostBuilder.RootComponents.Add<HeadOutlet>("head::after");

#endregion

var webAssemblyHost = hostBuilder.Build();

await webAssemblyHost.RunAsync().ConfigureAwait(false);
