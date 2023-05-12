using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using OnlineMinion.Web;
using OnlineMinion.Web.Settings;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

//-- Configurations setup
builder.Services.Configure<ApiClientSettings>(builder.Configuration.GetSection(nameof(ApiClientSettings)));

// Services setup
builder.Services.AddHttpClient(
    Constants.ApiClient,
    client => client.BaseAddress = new(builder.HostEnvironment.BaseAddress)
);

builder.Services.AddHttpClient(
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

//--
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//--
var webAssemblyHost = builder.Build();

await webAssemblyHost.RunAsync();
