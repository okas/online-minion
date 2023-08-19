using System.ComponentModel.DataAnnotations;
using MediatR;
using OnlineMinion.Contracts.PaymentSpec.Responses;

namespace OnlineMinion.Contracts.PaymentSpec.Requests;

public record GetPaymentSpecByIdReq([Required] int Id) : IHasIntId, IRequest<PaymentSpecResp?>;
