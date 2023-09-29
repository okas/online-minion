using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpec.Responses;
using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.PaymentSpec.Requests;

[UsedImplicitly]
public record GetPaymentSpecByIdReq([Required] Guid Id) : IGetByIdRequest<PaymentSpecResp>;
