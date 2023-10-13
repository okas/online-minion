using ErrorOr;
using FluentValidation;
using MediatR;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;
using OnlineMinion.Application.Contracts.Shared.Requests;
using OnlineMinion.Application.RequestValidation.Shared;

namespace OnlineMinion.Application.RequestValidation.PaymentSpecShared;

public abstract class BaseCheckUniqueExistingPaymentSpecNameReqValidator<TPaymentSpecModel>
    : BaseCheckUniqueModelValidator<TPaymentSpecModel>
    where TPaymentSpecModel : BaseUpsertPaymentSpecReqData, IUpdateCommand
{
    protected BaseCheckUniqueExistingPaymentSpecNameReqValidator(IAsyncValidatorSender sender) : base(sender)
    {
        RuleFor(x => x.Name)
            .MustAsync(BeUniqueAsync)
            .WithMessage(FailureMessageFormat);
    }

    protected override IRequest<ErrorOr<Success>> ValidationRequestFactory(TPaymentSpecModel model, string value) =>
        new CheckUniqueExistingPaymentSpecNameReq(value, model.Id);
}
