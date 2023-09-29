using ErrorOr;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Application.Contracts.PaymentSpec.Requests;
using OnlineMinion.Application.RequestValidation.Shared;
using OnlineMinion.Common;

namespace OnlineMinion.Application.RequestValidation.PaymentSpec;

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
