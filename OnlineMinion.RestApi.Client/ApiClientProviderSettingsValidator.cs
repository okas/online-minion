using FluentValidation;
using OnlineMinion.RestApi.Client.Settings;

namespace OnlineMinion.RestApi.Client;

public class ApiClientProviderSettingsValidator : AbstractValidator<ApiClientProviderSettings>
{
    public ApiClientProviderSettingsValidator()
    {
        RuleFor(x => x.Url)
            .Must(uri => Uri.TryCreate(uri, UriKind.RelativeOrAbsolute, out _))
            .WithName($"{nameof(ApiClientProviderSettings)}.{nameof(ApiClientProviderSettings.Url)}")
            .WithMessage(
                "'{PropertyName}' must be a valid URI, instead of '{PropertyValue}'; check appsettings/configuration."
            )
            .NotEmpty();
    }
}
