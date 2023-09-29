using ErrorOr;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Application.Contracts.AccountSpec.Requests;
using OnlineMinion.Application.RequestValidation.Shared;
using OnlineMinion.Common;

namespace OnlineMinion.Application.RequestValidation.AccountSpec;

[UsedImplicitly]
public sealed class CheckUniqueExistingAccountSpecReqValidator : BaseCheckUniqueModelValidator<UpdateAccountSpecReq>
{
    public CheckUniqueExistingAccountSpecReqValidator(IAsyncValidatorSender sender) : base(sender)
    {
        RuleFor(x => x.Name)
            .MustAsync(BeUniqueAsync)
            .WithMessage(FailureMessageFormat);
    }

    protected override string ModelName => "Account specification";

    protected override IRequest<ErrorOr<Success>> ValidationRequestFactory(UpdateAccountSpecReq model, string value) =>
        new CheckAccountSpecUniqueExistingReq(value, model.Id);
}
