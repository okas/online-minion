using ErrorOr;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Common.Validation.Shared;
using OnlineMinion.Contracts.PaymentSpec.Requests;

namespace OnlineMinion.Common.Validation.PaymentSpec;

[UsedImplicitly]
public sealed class CheckUniqueNewPaymentSpecValidator : BaseCheckUniqueModelValidator<CreatePaymentSpecReq>
{
    public CheckUniqueNewPaymentSpecValidator(IAsyncValidatorSender sender) : base(sender)
    {
        RuleFor(x => x.Name)
            .MustAsync(BeUniqueAsync)
            .WithMessage(FailureMessageFormat);
    }

    protected override string ModelName => "Payment specification";

    protected override IRequest<ErrorOr<Success>> ValidationRequestFactory(CreatePaymentSpecReq _, string value) =>
        new CheckPaymentSpecUniqueNewReq(value);
}
