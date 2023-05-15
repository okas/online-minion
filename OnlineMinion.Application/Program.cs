using CorsPolicySettings;
using MediatR;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OnlineMinion.Application.Swagger;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Data;
using OnlineMinion.Data.Entities;
using OnlineMinion.RestApi.AppMessaging.Handlers;
using OnlineMinion.RestApi.Configurators;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

var webAppBuilder = WebApplication.CreateBuilder(args);
var confManager = webAppBuilder.Configuration;

//-- Add services' and their configurations to the container.

webAppBuilder.Services.AddDbContext<OnlineMinionDbContext>(
    optionsBuilder => optionsBuilder.UseSqlServer(
        "name=ConnectionStrings:DefaultConnection",
        x => x.UseDateOnlyTimeOnly()
    )
);

// Order is important (CORS): first read base policies, then produce CORS configuration.
webAppBuilder.Services.AddCorsPolicies(confManager);
webAppBuilder.Services
    .AddSingleton<IConfigureOptions<CorsOptions>, ApiCorsOptionsConfigurator>()
    .AddCors();

webAppBuilder.Services.AddControllers();

webAppBuilder.Services
    .AddSingleton<IConfigureOptions<ApiVersioningOptions>, ApiVersioningOptionsConfigurator>()
    .AddApiVersioning();

webAppBuilder.Services
    .AddSingleton<IConfigureOptions<ApiExplorerOptions>, ApiExplorerOptionsConfigurator>()
    .AddVersionedApiExplorer();

webAppBuilder.Services.AddEndpointsApiExplorer();

webAppBuilder.Services
    .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(GetPagingInfoReqHlr<>)))
    .AddTransient<
        IRequestHandler<GetPagingMetaInfoReq<AccountSpec>, PagingMetaInfo>,
        GetPagingInfoReqHlr<AccountSpec>
    >();

if (webAppBuilder.Environment.IsDevelopment())
{
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

//--

var app = webAppBuilder.Build();

//-- Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//--

await app.RunAsync();
