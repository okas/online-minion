using ErrorOr;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.RestApi.Validation;

namespace OnlineMinion.RestApi.AccountSpec.Validation;

[UsedImplicitly]
public sealed class UpdateAccountSpecUniqueNameValidator : BaseCreateModelUniqueNameValidator<UpdateAccountSpecReq>
{
    public UpdateAccountSpecUniqueNameValidator(ISender sender) : base(sender)
    {
        RuleFor(x => x.Name)
            .MustAsync(BeUniqueAsync)
            .WithMessage(FailureMessageFormat);
    }

    protected override IRequest<ErrorOr<Success>> ValidationRequestFactory(UpdateAccountSpecReq model, string value) =>
        new CheckAccountSpecUniqueExistingReq(value, model.Id);
}
