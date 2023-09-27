using OnlineMinion.Contracts.Transactions.Debit;

namespace OnlineMinion.SPA.Blazor.Transaction.Debit.ViewModels;

/// <summary>
///     Purpose of this and its derived classes is to allow <paramref name="date" /> to have type <see cref="DateTime" />,
///     instead of <see cref="DateOnly" />.
/// </summary>
/// <remarks>
///     Need for this is because Radzen <see cref="Radzen.Blazor.RadzenDatePicker{TValue}" /> doesn't support
///     <see cref="DateOnly" />.
/// </remarks>
/// <param name="paymentInstrumentId"></param>
/// <param name="date"></param>
/// <param name="amount"></param>
/// <param name="subject"></param>
/// <param name="party"></param>
/// <param name="tags"></param>
public abstract class BaseTransactionDebitUpsertVM(
    int      paymentInstrumentId,
    int      accountSpecId,
    decimal  fee,
    DateTime date,
    decimal  amount,
    string   subject,
    string   party,
    string?  tags
) : BaseUpsertTransactionDebitReqData(
    paymentInstrumentId,
    accountSpecId,
    fee,
    DateOnly.FromDateTime(date),
    amount,
    subject,
    party,
    tags
)
{
    public new DateTime Date { get; set; } = date;
}
