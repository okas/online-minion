using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnlineMinion.Domain.AccountSpecs;

namespace OnlineMinion.DataStore.ValueConverters;

[UsedImplicitly]
public class AccountSpecIdConverter() : ValueConverter<AccountSpecId, Guid>(
    accountSpecId => accountSpecId.Value,
    value => new(value)
);
