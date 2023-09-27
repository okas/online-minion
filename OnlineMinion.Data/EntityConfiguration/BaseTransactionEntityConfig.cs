using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineMinion.Data.ValueGeneration;
using OnlineMinion.Domain.Shared;

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

        builder.Property(e => e.Tags)
            .HasMaxLength(150);

        builder.Property(e => e.CreatedAt)
            .HasValueGenerator<DefaultDateTimeValueGenerator>()
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);

        builder.HasOne(e => e.PaymentInstrument)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        builder.UseTpcMappingStrategy();
    }
}
