using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OnlineMinion.RestApi.Client.Configuration;
using OnlineMinion.Web;
using OnlineMinion.Web.Infrastructure;
using OnlineMinion.Web.Settings;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

#region Container setup

builder.Logging.AddConfiguration(
    builder.Configuration.GetSection("Logging")
);

builder.Services.AddHttpClient(
    Constants.HostClient,
    client => client.BaseAddress = new(builder.HostEnvironment.BaseAddress)
);

builder.Services.AddRestApiClient(
    builder.Configuration,
    new[] { new SetWebAssemblyStreamingOptionsHandler(), }
);

#endregion

#region Components setup

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

#endregion

var webAssemblyHost = builder.Build();

await webAssemblyHost.RunAsync().ConfigureAwait(false);
