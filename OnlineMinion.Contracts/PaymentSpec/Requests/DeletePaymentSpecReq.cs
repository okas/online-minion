using System.ComponentModel.DataAnnotations;
using ErrorOr;
using MediatR;

namespace OnlineMinion.Contracts.PaymentSpec.Requests;

public record DeletePaymentSpecReq([Required] int Id) : IHasIntId, IRequest<ErrorOr<Deleted>>;
