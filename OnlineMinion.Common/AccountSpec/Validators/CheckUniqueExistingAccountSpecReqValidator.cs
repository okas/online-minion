using ErrorOr;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Common.Shared.Validation;
using OnlineMinion.Contracts.AccountSpec.Requests;

namespace OnlineMinion.Common.AccountSpec.Validators;

[UsedImplicitly]
public sealed class CheckUniqueExistingAccountSpecReqValidator : BaseCheckUniqueModelValidator<UpdateAccountSpecReq>
{
    public CheckUniqueExistingAccountSpecReqValidator(ISender sender) : base(sender)
    {
        RuleFor(x => x.Name)
            .MustAsync(BeUniqueAsync)
            .WithMessage(FailureMessageFormat);
    }

    protected override string ModelName => "Account specification";

    protected override IRequest<ErrorOr<Success>> ValidationRequestFactory(UpdateAccountSpecReq model, string value) =>
        new CheckAccountSpecUniqueExistingReq(value, model.Id);
}
