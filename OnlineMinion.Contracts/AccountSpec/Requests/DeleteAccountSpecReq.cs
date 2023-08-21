using System.ComponentModel.DataAnnotations;
using OnlineMinion.Contracts.Requests;

namespace OnlineMinion.Contracts.AccountSpec.Requests;

public record DeleteAccountSpecReq([Required] int Id) : IDeleteByIdRequest;
