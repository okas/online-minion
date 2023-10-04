using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Domain.TransactionShared;

public abstract class BaseTransactionData
{
    public required DateOnly Date { get; set; }

    public required decimal Amount { get; set; }

    public required string Subject { get; set; }

    public required string Party { get; set; }

    public string? Tags { get; set; }

    public required PaymentSpecId PaymentInstrumentId { get; set; }

    public DateTime CreatedAt { get; set; }

    public BasePaymentSpec? PaymentInstrument { get; set; }
}
