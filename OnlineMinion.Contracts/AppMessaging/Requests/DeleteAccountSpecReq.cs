using System.ComponentModel.DataAnnotations;

namespace OnlineMinion.Contracts.AppMessaging.Requests;

public record DeleteAccountSpecReq([Required] int Id) : IDeleteByIdRequest;
