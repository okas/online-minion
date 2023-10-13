using ErrorOr;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Application.Contracts.PaymentSpecBank.Requests;
using OnlineMinion.Application.RequestValidation.Shared;

namespace OnlineMinion.Application.RequestValidation.PaymentSpecBank;

[UsedImplicitly]
public sealed class CheckUniqueNewPaymentSpecBankIBANValidator : BaseCheckUniqueModelValidator<CreatePaymentSpecBankReq>
{
    public CheckUniqueNewPaymentSpecBankIBANValidator(IAsyncValidatorSender sender) : base(sender)
    {
        RuleFor(x => x.IBAN)
            .MustAsync(BeUniqueAsync)
            .WithMessage(FailureMessageFormat);
    }

    protected override string ModelName => "Bank payment specification";

    protected override IRequest<ErrorOr<Success>> ValidationRequestFactory(CreatePaymentSpecBankReq _, string value) =>
        new CheckUniqueNewPaymentSpecBankIBANReq(value);
}
