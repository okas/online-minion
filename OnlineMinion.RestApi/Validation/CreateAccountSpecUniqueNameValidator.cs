using FluentValidation;
using MediatR;
using OnlineMinion.Contracts.AppMessaging.Requests;

namespace OnlineMinion.RestApi.Validation;

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
