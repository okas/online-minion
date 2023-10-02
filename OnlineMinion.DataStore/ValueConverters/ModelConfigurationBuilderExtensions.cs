using Microsoft.EntityFrameworkCore;
using OnlineMinion.Domain.AccountSpecs;
using OnlineMinion.Domain.PaymentSpecs;
using OnlineMinion.Domain.TransactionCredits;
using OnlineMinion.Domain.TransactionDebits;

namespace OnlineMinion.DataStore.ValueConverters;

public static class ModelConfigurationBuilderExtensions
{
    public static void ConfigureConverters(this ModelConfigurationBuilder builder)
    {
        builder.Properties<AccountSpecId>().HaveConversion<AccountSpecIdConverter>();
        builder.Properties<BasePaymentSpecId>().HaveConversion<PaymentSpecIdConverter>();
        builder.Properties<TransactionDebitId>().HaveConversion<TransactionDebitIdConverter>();
        builder.Properties<TransactionCreditId>().HaveConversion<TransactionCreditIdConverter>();
    }
}
