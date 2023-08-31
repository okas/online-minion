using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineMinion.Data.Entities;

namespace OnlineMinion.Data.EntityConfiguration;

public class TransactionDebitEntityConfig : IEntityTypeConfiguration<TransactionDebit>
{
    public void Configure(EntityTypeBuilder<TransactionDebit> builder)
    {
        builder.Property(e => e.Fee)
            .HasPrecision(18, 2);

        builder.HasOne(e => e.AccountSpec)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
