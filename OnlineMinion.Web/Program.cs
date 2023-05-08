using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OnlineMinion.Web;
using OnlineMinion.Web.HttpClients;
using OnlineMinion.Web.Settings;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

//-- Configurations setup
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection(nameof(ApiSettings)));

// Services setup
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<Api>();

//--
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//--
await builder.Build().RunAsync();
