using ErrorOr;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Common.Common.Validation;
using OnlineMinion.Contracts.PaymentSpec.Requests;

namespace OnlineMinion.Common.PaymentSpec.Validators;

[UsedImplicitly]
public sealed class CheckUniqueNewPaymentSpecValidator : BaseCheckUniqueModelValidator<CreatePaymentSpecReq>
{
    public CheckUniqueNewPaymentSpecValidator(ISender sender) : base(sender)
    {
        RuleFor(x => x.Name)
            .MustAsync(BeUniqueAsync)
            .WithMessage(FailureMessageFormat);
    }

    protected override string ModelName => "Payment specification";

    protected override IRequest<ErrorOr<Success>> ValidationRequestFactory(CreatePaymentSpecReq _, string value) =>
        new CheckPaymentSpecUniqueNewReq(value);
}
