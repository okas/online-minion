using ErrorOr;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.RestApi.Validation;

namespace OnlineMinion.RestApi.PaymentSpec.Validation;

[UsedImplicitly]
public sealed class UpdatePaymentSpecUniqueNameValidator : BaseCreateModelUniqueNameValidator<UpdatePaymentSpecReq>
{
    public UpdatePaymentSpecUniqueNameValidator(ISender sender) : base(sender)
    {
        RuleFor(x => x.Name)
            .MustAsync(BeUniqueAsync)
            .WithMessage(FailureMessageFormat);
    }

    protected override IRequest<ErrorOr<Success>> ValidationRequestFactory(UpdatePaymentSpecReq model, string value) =>
        new CheckPaymentSpecUniqueExistingReq(value, model.Id);
}
