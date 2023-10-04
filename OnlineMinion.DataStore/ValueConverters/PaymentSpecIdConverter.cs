using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.DataStore.ValueConverters;

[UsedImplicitly]
public class PaymentSpecIdConverter() : ValueConverter<PaymentSpecId, Guid>(
    paymentSpecId => paymentSpecId.Value,
    value => new(value)
);