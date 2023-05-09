using CorsPolicySettings;
using MediatR;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OnlineMinion.API.Configurators;
using OnlineMinion.API.Swagger;
using OnlineMinion.Common.CQRS.QueryHandlers;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.Queries;
using OnlineMinion.Data;
using OnlineMinion.Data.Entities;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

var webAppBuilder = WebApplication.CreateBuilder(args);

//-- Add services' configurations to the container.
// Order is important (CORS): first read base policies, then produce CORS configuration.
webAppBuilder.Services.AddCorsPolicies(webAppBuilder.Configuration);
webAppBuilder.Services.AddSingleton<IConfigureOptions<CorsOptions>, ApiCorsOptionsConfigurator>();

webAppBuilder.Services
    .AddSingleton<IConfigureOptions<ApiVersioningOptions>, ApiVersioningOptionsConfigurator>()
    .AddSingleton<IConfigureOptions<ApiExplorerOptions>, ApiExplorerOptionsConfigurator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
webAppBuilder.Services
    .AddSingleton<IConfigureOptions<SwaggerGenOptions>, SwaggerGenOptionsConfiguration>()
    .AddSingleton<IConfigureOptions<SwaggerUIOptions>, SwaggerUIOptionsConfigurator>();

//-- Add services to the container.
webAppBuilder.Services.AddDbContext<OnlineMinionDbContext>(
    optionsBuilder => optionsBuilder.UseSqlServer(
        "name=ConnectionStrings:DefaultConnection",
        x => x.UseDateOnlyTimeOnly()
    )
);

webAppBuilder.Services.AddMediatR(
        cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(GetPagingInfoQryHlr<>))
    )
    .AddTransient<
        IRequestHandler<GetPagingMetaInfoQry<AccountSpec>, PagingMetaInfo>,
        GetPagingInfoQryHlr<AccountSpec>
    >();

webAppBuilder.Services.AddCors();
webAppBuilder.Services.AddControllers();
webAppBuilder.Services.AddApiVersioning().AddVersionedApiExplorer();
webAppBuilder.Services.AddEndpointsApiExplorer();
if (webAppBuilder.Environment.IsDevelopment())
{
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
