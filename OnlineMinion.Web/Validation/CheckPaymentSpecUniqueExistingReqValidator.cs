using ErrorOr;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Common;
using OnlineMinion.Contracts.PaymentSpec.Requests;

namespace OnlineMinion.Web.Validation;

[UsedImplicitly]
public class CheckPaymentSpecUniqueExistingReqValidator
    : AbstractValidator<UpdatePaymentSpecReq>, IAsyncUniqueValidator<UpdatePaymentSpecReq>
{
    private readonly ISender _sender;

    public CheckPaymentSpecUniqueExistingReqValidator(ISender sender)
    {
        _sender = sender;

        RuleFor(x => x.Name)
            .MustAsync(BeUniqueAsync)
            .WithMessage("'{PropertyName}' must be unique");
    }

    private async Task<bool> BeUniqueAsync(UpdatePaymentSpecReq model, string name, CancellationToken ct)
    {
        var uniquenessRq = new CheckPaymentSpecUniqueExistingReq(name, model.Id);
        var result = await _sender.Send(uniquenessRq, ct).ConfigureAwait(false);

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
