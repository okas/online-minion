using OnlineMinion.RestApi.Init;

const string connectionString = "name=ConnectionStrings:DefaultConnection";

var webAppBuilder = WebApplication.CreateBuilder(args);

#region Container setup

webAppBuilder.Services.AddDataStore(connectionString);

webAppBuilder.Services.AddRestApi(webAppBuilder.Configuration, webAppBuilder.Environment);

if (webAppBuilder.Environment.IsDevelopment())
{
    webAppBuilder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

#endregion

var webApp = webAppBuilder.Build();

#region HTTP request pipeline

webApp.UseHttpsRedirection();

webApp.UseRestApi();

#endregion

await webApp.RunAsync().ConfigureAwait(false);
