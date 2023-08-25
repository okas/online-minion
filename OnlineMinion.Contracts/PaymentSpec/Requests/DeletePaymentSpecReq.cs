using System.ComponentModel.DataAnnotations;
using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.Contracts.PaymentSpec.Requests;

public record DeletePaymentSpecReq([Required] int Id) : IDeleteByIdRequest;
