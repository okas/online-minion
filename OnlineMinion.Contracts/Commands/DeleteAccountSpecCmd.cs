using System.ComponentModel.DataAnnotations;
using MediatR;

namespace OnlineMinion.Contracts.Commands;

public record struct DeleteAccountSpecCmd([property: Required] int Id) : IRequest<int>;
