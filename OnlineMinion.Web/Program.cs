using FluentValidation;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OnlineMinion.Common.Validators;
using OnlineMinion.RestApi.Client.Configuration;
using OnlineMinion.Web;
using OnlineMinion.Web.Configuration;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

#region Container setup

var services = builder.Services;

builder.Logging.AddConfiguration(
    builder.Configuration.GetSection("Logging")
);

services.AddLogging();

services.AddHttpClient(
    Constants.HostClient,
    client => client.BaseAddress = new(builder.HostEnvironment.BaseAddress)
);

services.AddRestApiClient(
    new[] { typeof(SetWebAssemblyStreamingOptionsHttpRequestHandler), },
    Constants.ApiClient
);

services.AddSingleton<StateContainer>();

services.AddValidatorsFromAssemblyContaining<Program>();
services.AddValidatorsFromAssemblyContaining<HasIntIdValidator>();

services.AddScoped<DialogService>();

#endregion

#region Components setup

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

#endregion

var webAssemblyHost = builder.Build();

await webAssemblyHost.RunAsync().ConfigureAwait(false);
