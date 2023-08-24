using ErrorOr;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Common.Common.Validation;
using OnlineMinion.Contracts.AccountSpec.Requests;

namespace OnlineMinion.Common.AccountSpec.Validators;

[UsedImplicitly]
public sealed class CheckUniqueNewAccountSpecValidator : BaseCheckUniqueModelValidator<CreateAccountSpecReq>
{
    public CheckUniqueNewAccountSpecValidator(ISender sender) : base(sender)
    {
        RuleFor(x => x.Name)
            .MustAsync(BeUniqueAsync)
            .WithMessage(FailureMessageFormat);
    }

    protected override string ModelName => "Account specification";

    protected override IRequest<ErrorOr<Success>> ValidationRequestFactory(CreateAccountSpecReq _, string value) =>
        new CheckAccountSpecUniqueNewReq(value);
}
