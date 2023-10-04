using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using OnlineMinion.RestApi.ProblemHandling;

namespace OnlineMinion.RestApi.Init;

public static class HttpRequestPipelineSetup
{
    public static WebApplication UseRestApi(this WebApplication app)
    {
        app.UseSwagger();

        if (app.Environment.IsDevelopment())
        {
            // Required to serve custom CSS for SwaggerUI.
            app.UseStaticFiles();

            app.UseSwaggerUI();
        }

        app.UseCors();

        app.UseExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            ErrorEndpoints.MapDevEndpoints(app);
        }

        CurrencyInfoEndpoints.MapAll(app);
        AccountSpecsEndpoints.MapAll(app);
        PaymentCacheSpecsEndpoints.MapAll(app);
        TransactionCreditsEndpoints.MapAll(app);
        TransactionDebitsEndpoints.MapAll(app);

        return app;
    }
}
