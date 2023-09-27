using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.Contracts.PaymentSpec.Requests;

[UsedImplicitly]
public record GetPaymentSpecByIdReq([Required] int Id) : IGetByIdRequest<PaymentSpecResp>;
