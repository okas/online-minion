using System.ComponentModel.DataAnnotations;
using MediatR;

namespace OnlineMinion.Contracts.AppMessaging.Requests;

public record DeleteAccountSpecReq(
    [Required(DisallowAllDefaultValues = true)]
    int Id
) : IHasIntId, IRequest<bool>;
