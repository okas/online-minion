using ErrorOr;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Application.Contracts.PaymentSpecCash.Requests;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;
using OnlineMinion.Application.RequestValidation.Shared;

namespace OnlineMinion.Application.RequestValidation.PaymentSpecShared;

[UsedImplicitly]
public sealed class CheckUniqueExistingPaymentSpecReqValidator : BaseCheckUniqueModelValidator<UpdatePaymentSpecCashReq>
{
    public CheckUniqueExistingPaymentSpecReqValidator(IAsyncValidatorSender sender) : base(sender)
    {
        RuleFor(x => x.Name)
            .MustAsync(BeUniqueAsync)
            .WithMessage(FailureMessageFormat);
    }

    protected override string ModelName => "Payment specification";

    protected override IRequest<ErrorOr<Success>> ValidationRequestFactory(
        UpdatePaymentSpecCashReq model,
        string                   value
    ) =>
        new CheckPaymentSpecUniqueExistingReq(value, model.Id);
}
