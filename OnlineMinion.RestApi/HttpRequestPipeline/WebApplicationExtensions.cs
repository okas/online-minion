using Microsoft.AspNetCore.Builder;

namespace OnlineMinion.RestApi.HttpRequestPipeline;

public static class WebApplicationExtensions
{
    public static WebApplication UseRestApi(this WebApplication app)
    {
        app.UseCors();

        app.UseAuthorization();

        app.UseExceptionHandler("/error");

        app.MapControllers();

        return app;
    }
}
