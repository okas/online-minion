using ErrorOr;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Common.Common.Validation;
using OnlineMinion.Contracts.PaymentSpec.Requests;

namespace OnlineMinion.Common.PaymentSpec.Validators;

[UsedImplicitly]
public sealed class CheckUniqueExistingPaymentSpecReqValidator : BaseCheckUniqueModelValidator<UpdatePaymentSpecReq>
{
    public CheckUniqueExistingPaymentSpecReqValidator(ISender sender) : base(sender)
    {
        RuleFor(x => x.Name)
            .MustAsync(BeUniqueAsync)
            .WithMessage(FailureMessageFormat);
    }

    protected override string ModelName => "Payment specification";

    protected override IRequest<ErrorOr<Success>> ValidationRequestFactory(UpdatePaymentSpecReq model, string value) =>
        new CheckPaymentSpecUniqueExistingReq(value, model.Id);
}
