using ErrorOr;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Common;
using OnlineMinion.Contracts.PaymentSpec.Requests;

namespace OnlineMinion.Web.Validation;

[UsedImplicitly]
public sealed class CreatePaymentSpecUniqueNameValidator
    : AbstractValidator<CreatePaymentSpecReq>, IAsyncUniqueValidator<CreatePaymentSpecReq>
{
    private readonly ISender _sender;

    public CreatePaymentSpecUniqueNameValidator(ISender sender)
    {
        _sender = sender;

        RuleFor(x => x.Name)
            .MustAsync(BeUniqueAsync)
            .WithMessage("'{PropertyName}' must be unique");
    }

    private async Task<bool> BeUniqueAsync(string name, CancellationToken ct)
    {
        var rq = new CheckPaymentSpecUniqueNewReq(name);
        var result = await _sender.Send(rq, ct).ConfigureAwait(false);

        return result.MatchFirst(
            _ => true,
            firstError => firstError.Type is ErrorType.Conflict
                ? false
                : throw new ValidationException(
                    "Unexpected error while checking uniqueness of Payment specification name"
                )
        );
    }
}
