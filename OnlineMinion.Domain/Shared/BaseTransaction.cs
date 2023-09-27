namespace OnlineMinion.Domain.Shared;

public abstract class BaseTransaction : BaseEntity
{
    public required DateOnly Date { get; set; }

    public required decimal Amount { get; set; }

    public required string Subject { get; set; }

    public required string Party { get; set; }

    public string? Tags { get; set; }

    public required Guid PaymentInstrumentId { get; set; }

    public DateTime CreatedAt { get; set; }

    public BasePaymentSpec? PaymentInstrument { get; set; }
}
