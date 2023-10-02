using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineMinion.Domain.TransactionCredits;

namespace OnlineMinion.DataStore.EntityConfiguration;

public class TransactionCreditEntityConfig : IEntityTypeConfiguration<TransactionCredit>
{
    public void Configure(EntityTypeBuilder<TransactionCredit> builder)
    {
        CommonTransactionEntityConfig.ConfigureCommonTransaction(builder);

        builder.HasKey(e => e.Id);
    }
}
