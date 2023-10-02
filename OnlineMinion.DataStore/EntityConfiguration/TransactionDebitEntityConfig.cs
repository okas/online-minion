using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineMinion.Domain.TransactionDebits;

namespace OnlineMinion.DataStore.EntityConfiguration;

public class TransactionDebitEntityConfig : IEntityTypeConfiguration<TransactionDebit>
{
    public void Configure(EntityTypeBuilder<TransactionDebit> builder)
    {
        CommonTransactionEntityConfig.ConfigureCommonTransaction(builder);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Fee)
            .HasPrecision(18, 2);

        builder.HasOne(e => e.AccountSpec)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
