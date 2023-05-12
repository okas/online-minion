using System.ComponentModel.DataAnnotations;
using MediatR;

namespace OnlineMinion.Contracts.Commands;

public record DeleteAccountSpecCmd([Required] int Id) : IRequest<int>;
