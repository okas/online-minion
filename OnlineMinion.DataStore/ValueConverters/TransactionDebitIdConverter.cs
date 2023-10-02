using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnlineMinion.Domain.TransactionDebits;

namespace OnlineMinion.DataStore.ValueConverters;

[UsedImplicitly]
public class TransactionDebitIdConverter() : ValueConverter<TransactionDebitId, Guid>(
    transactionDebitId => transactionDebitId.Value,
    value => new(value)
);
