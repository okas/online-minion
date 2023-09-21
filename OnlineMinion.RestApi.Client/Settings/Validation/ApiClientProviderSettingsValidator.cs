using FluentValidation;
using JetBrains.Annotations;

namespace OnlineMinion.RestApi.Client.Settings.Validation;

[UsedImplicitly]
public class ApiProviderSettingsValidator : AbstractValidator<ApiProviderSettings>
{
    private const string TypeName = nameof(ApiProviderSettings);

    public ApiProviderSettingsValidator()
    {
        RuleFor(x => x.Url)
            .Must(uri => Uri.TryCreate(uri, UriKind.RelativeOrAbsolute, out _))
            .WithName($"{TypeName}.{nameof(ApiProviderSettings.Url)}")
            .WithMessage(
                "'{PropertyName}' must be a valid URI, instead of '{PropertyValue}'; check appsettings/configuration."
            )
            .NotEmpty();

        RuleFor(x => x.DefaultApiVersion)
            .MinimumLength(1)
            .WithName($"{TypeName}.{nameof(ApiProviderSettings.DefaultApiVersion)}")
            .WithMessage(
                "'{PropertyName}' must be a valid API version, instead of '{PropertyValue}'; check appsettings/configuration."
            );
    }
}
