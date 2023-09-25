using Microsoft.AspNetCore.Builder;

namespace OnlineMinion.RestApi.Init;

public static class HttpRequestPipelineSetup
{
    public static WebApplication UseRestApi(this WebApplication app)
    {
        app.UseCors();

        app.UseExceptionHandler("/error");

        CurrencyInfoEndpoints.MapAll(app);
        AccountSpecsEndpoints.MapAll(app);
        PaymentSpecsEndpoints.MapAll(app);
        TransactionCreditsEndpoints.MapAll(app);
        TransactionDebitsEndpoints.MapAll(app);

        return app;
    }
}
