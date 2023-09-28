using OnlineMinion.RestApi.Init;

const string connectionString = "name=ConnectionStrings:DefaultConnection";

var webAppBuilder = WebApplication.CreateBuilder(args);

#region Container setup

var confManager = webAppBuilder.Configuration;
var services = webAppBuilder.Services;

services.AddDataStore(connectionString)
    .AddRestApi(webAppBuilder.Configuration);

services.AddSwaggerGen(confManager);

if (webAppBuilder.Environment.IsDevelopment())
{
    services.AddDatabaseDeveloperPageExceptionFilter();

    services.AddSwaggerUI(confManager);
}

#endregion

var app = webAppBuilder.Build();

#region HTTP request pipeline

app.UseSwagger();

if (app.Environment.IsDevelopment())
{
    // Required to serve custom CSS for SwaggerUI.
    app.UseStaticFiles();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRestApi();

#endregion

await app.RunAsync().ConfigureAwait(false);
