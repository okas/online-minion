using FluentValidation;
using MediatR;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.RestApi.Validation;

namespace OnlineMinion.RestApi.PaymentSpec.Validation;

public sealed class UpdatePaymentSpecUniqueNameValidator : BaseCreateModelUniqueNameValidator<UpdatePaymentSpecReq>
{
    public UpdatePaymentSpecUniqueNameValidator(ISender sender) : base(sender)
    {
        RuleFor(x => x.Name)
            .MustAsync(BeUnique)
            .WithMessage(FailureMessageFormat);
    }

    protected override IRequest<bool> ValidationRequestFactory(UpdatePaymentSpecReq model, string value) =>
        new CheckPaymentSpecUniqueExistingReq(value, model.Id);
}
