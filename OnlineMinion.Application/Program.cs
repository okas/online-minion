using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OnlineMinion.Application.Swagger;
using OnlineMinion.Data;
using OnlineMinion.RestApi.Configuration;
using OnlineMinion.RestApi.HttpRequestPipeline;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

var webAppBuilder = WebApplication.CreateBuilder(args);

#region Container setup

var confManager = webAppBuilder.Configuration;

//-- Add services' and their configurations to the container.

webAppBuilder.Services.AddDbContext<OnlineMinionDbContext>(
    optionsBuilder => optionsBuilder.UseSqlServer(
        "name=ConnectionStrings:DefaultConnection",
        x => x.UseDateOnlyTimeOnly()
    )
);

if (webAppBuilder.Environment.IsDevelopment())
{
    webAppBuilder.Services.AddDatabaseDeveloperPageExceptionFilter();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    webAppBuilder.Services.Configure<SwaggerOptions>(confManager.GetSection(nameof(SwaggerOptions)));

    webAppBuilder.Services
        .Configure<SwaggerGeneratorOptions>(confManager.GetSection(nameof(SwaggerGeneratorOptions)))
        .Configure<SchemaGeneratorOptions>(confManager.GetSection(nameof(SchemaGeneratorOptions)))
        .Configure<SwaggerGenOptions>(confManager.GetSection(nameof(SwaggerGenOptions)))
        .AddSingleton<IConfigureOptions<SwaggerGenOptions>, SwaggerGenOptionsConfiguration>();

    webAppBuilder.Services
        .Configure<SwaggerUIOptions>(confManager.GetSection(nameof(SwaggerUIOptions)))
        .AddSingleton<IConfigureOptions<SwaggerUIOptions>, SwaggerUIOptionsConfigurator>();

    webAppBuilder.Services.AddSwaggerGen();
}

webAppBuilder.Services.AddRestApi(webAppBuilder.Configuration);

#endregion

var app = webAppBuilder.Build();

#region HTTP request pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRestApi();

#endregion

await app.RunAsync();
