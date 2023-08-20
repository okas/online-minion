using FluentValidation;
using MediatR;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.RestApi.Validation;

namespace OnlineMinion.RestApi.PaymentSpec.Validation;

public sealed class CreatePaymentSpecUniqueNameValidator : BaseCreateModelUniqueNameValidator<CreatePaymentSpecReq>
{
    public CreatePaymentSpecUniqueNameValidator(ISender sender) : base(sender)
    {
        RuleFor(x => x.Name)
            .MustAsync(BeUnique)
            .WithMessage(FailureMessageFormat);
    }

    protected override IRequest<bool> ValidationRequestFactory(CreatePaymentSpecReq model, string value) =>
        new CheckPaymentSpecUniqueNewReq(value);
}
