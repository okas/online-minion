using FluentValidation;
using MediatR;
using OnlineMinion.Contracts.AppMessaging.Requests;

namespace OnlineMinion.RestApi.Validation;

public sealed class UpdateAccountSpecUniqueNameValidator : BaseCreateModelUniqueNameValidator<UpdateAccountSpecReq>
{
    public UpdateAccountSpecUniqueNameValidator(ISender sender) : base(sender)
    {
        RuleFor(x => x.Name)
            .MustAsync(BeUnique)
            .WithMessage(FailureMessageFormat);
    }

    protected override IRequest<bool> ValidationRequestFactory(UpdateAccountSpecReq model, string value) =>
        new CheckAccountSpecUniqueExistingReq(value, model.Id);
}
