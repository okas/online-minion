using ErrorOr;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.RestApi.Validation;

namespace OnlineMinion.RestApi.AccountSpec.Validation;

[UsedImplicitly]
public sealed class CreateAccountSpecUniqueNameValidator : BaseCreateModelUniqueNameValidator<CreateAccountSpecReq>
{
    public CreateAccountSpecUniqueNameValidator(ISender sender) : base(sender)
    {
        RuleFor(x => x.Name)
            .MustAsync(BeUniqueAsync)
            .WithMessage(FailureMessageFormat);
    }

    protected override IRequest<ErrorOr<Success>> ValidationRequestFactory(CreateAccountSpecReq _, string value) =>
        new CheckAccountSpecUniqueNewReq(value);
}
