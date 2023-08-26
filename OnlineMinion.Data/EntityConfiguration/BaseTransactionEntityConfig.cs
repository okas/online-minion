using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineMinion.Data.Entities.Shared;

namespace OnlineMinion.Data.EntityConfiguration;

public class BaseTransactionEntityConfig : IEntityTypeConfiguration<BaseTransaction>
{
    public void Configure(EntityTypeBuilder<BaseTransaction> builder)
    {
        builder.Property(e => e.Subject)
            .HasMaxLength(50);

        builder.Property(e => e.Party)
            .HasMaxLength(75);

        builder.Property(e => e.Amount)
            .HasPrecision(18, 2);

        builder.HasOne(e => e.PaymentInstrument)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);

        builder.UseTpcMappingStrategy();
    }
}
