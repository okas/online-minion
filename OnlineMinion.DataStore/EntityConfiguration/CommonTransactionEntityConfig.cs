using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineMinion.DataStore.ValueGeneration;
using OnlineMinion.Domain.TransactionShared;

namespace OnlineMinion.DataStore.EntityConfiguration;

public static class CommonTransactionEntityConfig
{
    public static void ConfigureCommonTransaction<TEntity>(EntityTypeBuilder<TEntity> builder)
        where TEntity : BaseTransactionData
    {
        builder.UseTpcMappingStrategy();

        builder.Property(e => e.Subject)
            .HasMaxLength(50);

        builder.Property(e => e.Party)
            .HasMaxLength(75);

        builder.Property(e => e.Amount)
            .HasPrecision(18, 2);

        builder.Property(e => e.Tags)
            .HasMaxLength(150);

        builder.Property(e => e.CreatedAt)
            .HasValueGenerator<DefaultDateTimeValueGenerator>()
            .ValueGeneratedOnAdd()
            .Metadata
            .SetAfterSaveBehavior(PropertySaveBehavior.Save);

        builder.HasOne(e => e.PaymentInstrument)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
