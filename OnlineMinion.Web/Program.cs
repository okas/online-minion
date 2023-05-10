using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OnlineMinion.Web;
using OnlineMinion.Web.HttpClients;
using OnlineMinion.Web.Settings;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

//-- Configurations setup
builder.Services.Configure<ApiClientSettings>(builder.Configuration.GetSection(nameof(ApiClientSettings)));

// Services setup
builder.Services.AddHttpClient<HostHttpClient>();
builder.Services.AddHttpClient<ApiHttpClient>();

builder.Services.AddMediatR(
    opts => opts.RegisterServicesFromAssemblyContaining<Program>()
);

//--
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//--
var webAssemblyHost = builder.Build();

await webAssemblyHost.RunAsync();
