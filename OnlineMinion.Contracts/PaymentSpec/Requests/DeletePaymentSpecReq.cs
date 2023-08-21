using System.ComponentModel.DataAnnotations;
using OnlineMinion.Contracts.AppMessaging;

namespace OnlineMinion.Contracts.PaymentSpec.Requests;

public record DeletePaymentSpecReq([Required] int Id) : IDeleteByIdRequest;
