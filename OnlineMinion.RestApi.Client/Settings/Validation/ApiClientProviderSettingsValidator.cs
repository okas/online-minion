using FluentValidation;
using JetBrains.Annotations;

namespace OnlineMinion.RestApi.Client.Settings.Validation;

[UsedImplicitly]
public class ApiClientSettingsValidator : AbstractValidator<ApiClientSettings>
{
    private const string TypeName = nameof(ApiClientSettings);

    public ApiClientSettingsValidator()
    {
        RuleFor(x => x.Url)
            .Must(uri => Uri.TryCreate(uri, UriKind.RelativeOrAbsolute, out _))
            .WithName($"{TypeName}.{nameof(ApiClientSettings.Url)}")
            .WithMessage(
                "'{PropertyName}' must be a valid URI, instead of '{PropertyValue}'; check appsettings/configuration."
            )
            .NotEmpty();
    }
}
