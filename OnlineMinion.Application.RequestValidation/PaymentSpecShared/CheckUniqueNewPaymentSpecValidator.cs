using ErrorOr;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Application.Contracts.PaymentSpecCash.Requests;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;
using OnlineMinion.Application.RequestValidation.Shared;

namespace OnlineMinion.Application.RequestValidation.PaymentSpecShared;

[UsedImplicitly]
public sealed class CheckUniqueNewPaymentSpecValidator : BaseCheckUniqueModelValidator<CreatePaymentSpecCashReq>
{
    public CheckUniqueNewPaymentSpecValidator(IAsyncValidatorSender sender) : base(sender)
    {
        RuleFor(x => x.Name)
            .MustAsync(BeUniqueAsync)
            .WithMessage(FailureMessageFormat);
    }

    protected override string ModelName => "Payment specification";

    protected override IRequest<ErrorOr<Success>> ValidationRequestFactory(CreatePaymentSpecCashReq _, string value) =>
        new CheckPaymentSpecUniqueNewReq(value);
}
