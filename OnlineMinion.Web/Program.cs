using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
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

builder.Services
    .Configure<ApiServiceSettings>(builder.Configuration.GetSection(nameof(ApiServiceSettings)))
    .AddHttpClient<ApiService>();

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
