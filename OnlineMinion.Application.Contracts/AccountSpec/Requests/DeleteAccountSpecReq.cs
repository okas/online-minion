using System.ComponentModel.DataAnnotations;
using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.AccountSpec.Requests;

public record DeleteAccountSpecReq([Required] Guid Id) : IDeleteByIdCommand;
