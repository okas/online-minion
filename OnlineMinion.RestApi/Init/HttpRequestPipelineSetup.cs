using Microsoft.AspNetCore.Builder;

namespace OnlineMinion.RestApi.Init;

public static class HttpRequestPipelineSetup
{
    public static WebApplication UseRestApi(this WebApplication app)
    {
        app.UseCors();

        app.UseAuthorization();

        app.UseExceptionHandler("/error");

        app.MapControllers();

        CurrencyInfoEndpoints.MapAll(app);
        AccountSpecsEndpoints.MapAll(app);
        PaymentSpecsEndpoints.MapAll(app);

        return app;
    }
}
