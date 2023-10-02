using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnlineMinion.Domain.TransactionCredits;

namespace OnlineMinion.DataStore.ValueConverters;

[UsedImplicitly]
public class TransactionCreditIdConverter() : ValueConverter<TransactionCreditId, Guid>(
    transactionCreditId => transactionCreditId.Value,
    value => new(value)
);
