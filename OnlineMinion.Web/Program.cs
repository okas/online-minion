using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using OnlineMinion.Web;
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

builder.Services
    .Configure<ApiClientSettings>(builder.Configuration.GetSection(nameof(ApiClientSettings)))
    .AddHttpClient(
        Constants.ApiClient,
        (provider, client) =>
            client.BaseAddress = new(
                provider.GetService<IOptions<ApiClientSettings>>()?.Value.Url ??
                throw new InvalidOperationException($"Missing {nameof(ApiClientSettings)} from configuration.")
            )
    );

builder.Services.AddMediatR(
    opts => opts.RegisterServicesFromAssemblyContaining<Program>()
);

#endregion

#region Components setup

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

#endregion

var webAssemblyHost = builder.Build();

await webAssemblyHost.RunAsync().ConfigureAwait(false);
