using ErrorOr;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Application.Contracts.AccountSpec.Requests;
using OnlineMinion.Application.RequestValidation.Shared;

namespace OnlineMinion.Application.RequestValidation.AccountSpec;

[UsedImplicitly]
public sealed class CheckUniqueNewAccountSpecValidator : BaseCheckUniqueModelValidator<CreateAccountSpecReq>
{
    public CheckUniqueNewAccountSpecValidator(IAsyncValidatorSender sender) : base(sender)
    {
        RuleFor(x => x.Name)
            .MustAsync(BeUniqueAsync)
            .WithMessage(FailureMessageFormat);
    }

    protected override string ModelName => "Account specification";

    protected override IRequest<ErrorOr<Success>> ValidationRequestFactory(CreateAccountSpecReq _, string value) =>
        new CheckAccountSpecUniqueNewReq(value);
}
