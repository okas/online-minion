using ErrorOr;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Common;
using OnlineMinion.Contracts.AccountSpec.Requests;

namespace OnlineMinion.Web.Validation;

[UsedImplicitly]
public sealed class CheckUniqueExistingAccountSpecReqValidator : AbstractValidator<UpdateAccountSpecReq>,
    IAsyncUniqueValidator<UpdateAccountSpecReq>
{
    private readonly ISender _sender;

    public CheckUniqueExistingAccountSpecReqValidator(ISender sender)
    {
        _sender = sender;

        RuleFor(x => x.Name)
            .MustAsync(BeUniqueAsync)
            .WithMessage("'{PropertyName}' must be unique");
    }

    private async Task<bool> BeUniqueAsync(UpdateAccountSpecReq model, string name, CancellationToken ct)
    {
        var rq = new CheckAccountSpecUniqueExistingReq(name, model.Id);
        var result = await _sender.Send(rq, ct).ConfigureAwait(false);

        return result.MatchFirst(
            _ => true,
            firstError => firstError.Type is ErrorType.Conflict
                ? false
                : throw new ValidationException(
                    "Unexpected error while checking uniqueness of Account specification name"
                )
        );
    }
}
