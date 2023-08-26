using System.ComponentModel.DataAnnotations;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.Contracts.PaymentSpec.Requests;

public record GetPaymentSpecByIdReq([Required] int Id) : IGetByIdRequest<PaymentSpecResp?>;
