using System.ComponentModel.DataAnnotations;
using OnlineMinion.Contracts.Common.Requests;

namespace OnlineMinion.Contracts.PaymentSpec.Requests;

public record DeletePaymentSpecReq([Required] int Id) : IDeleteByIdRequest;
