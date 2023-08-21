using FluentValidation;
using MediatR;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.RestApi.Validation;

namespace OnlineMinion.RestApi.AccountSpec.Validation;

public sealed class CreateAccountSpecUniqueNameValidator : BaseCreateModelUniqueNameValidator<CreateAccountSpecReq>
{
    public CreateAccountSpecUniqueNameValidator(ISender sender) : base(sender)
    {
        RuleFor(x => x.Name)
            .MustAsync(BeUnique)
            .WithMessage(FailureMessageFormat);
    }

    protected override IRequest<bool> ValidationRequestFactory(CreateAccountSpecReq model, string value) =>
        new CheckAccountSpecUniqueNewReq(value);
}
