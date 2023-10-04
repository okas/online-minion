using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Responses;
using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;

[UsedImplicitly]
public record GetByIdPaymentSpecReq([Required] Guid Id) : IGetByIdRequest<PaymentSpecResp>;
