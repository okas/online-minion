using System.ComponentModel.DataAnnotations;
using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;

public record DeletePaymentSpecReq([Required] Guid Id) : IDeleteByIdCommand;
