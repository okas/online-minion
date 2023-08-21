using FluentValidation;
using MediatR;
using OnlineMinion.Common;
using OnlineMinion.Contracts.AccountSpec.Requests;

namespace OnlineMinion.Web.Validation;

public class CheckAccountSpecUniqueExistingReqValidator : AbstractValidator<UpdateAccountSpecReq>,
    IAsyncUniqueValidator<UpdateAccountSpecReq>
{
    private readonly ISender _sender;

    public CheckAccountSpecUniqueExistingReqValidator(ISender sender)
    {
        _sender = sender;

        RuleFor(x => x.Name)
            .MustAsync(BeUnique)
            .WithMessage("'{PropertyName}' must be unique");
    }

    private async Task<bool> BeUnique(UpdateAccountSpecReq req, string name, CancellationToken ct) =>
        await _sender.Send(new CheckAccountSpecUniqueExistingReq(name, req.Id), ct).ConfigureAwait(false);
}
