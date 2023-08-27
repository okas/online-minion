using System.ComponentModel.DataAnnotations;
using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.Contracts.AccountSpec.Requests;

public record DeleteAccountSpecReq([Required] int Id) : IDeleteByIdCommand;
