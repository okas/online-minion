using FluentValidation;
using JetBrains.Annotations;

namespace OnlineMinion.SPA.Blazor.Settings.Validation;

[UsedImplicitly]
public class WebAppSettingsValidator : AbstractValidator<WebAppSettings>
{
    private const string TypeName = nameof(WebAppSettings);

    public WebAppSettingsValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.BrowserResponseStreamingEnabled)
            .NotEmpty()
            .WithName($"{TypeName}.{nameof(WebAppSettings.BrowserResponseStreamingEnabled)}")
            .WithMessage(
                "'{PropertyName}' must be a valid boolean, instead of '{PropertyValue}'; check appsettings/configuration."
            );
    }
}
