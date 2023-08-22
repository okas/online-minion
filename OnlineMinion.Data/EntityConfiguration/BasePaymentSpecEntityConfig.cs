using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineMinion.Data.BaseEntities;

namespace OnlineMinion.Data.EntityConfiguration;

public class BasePaymentSpecEntityConfig : IEntityTypeConfiguration<BasePaymentSpec>
{
    public void Configure(EntityTypeBuilder<BasePaymentSpec> builder)
    {
        builder.Property(e => e.Name)
            .HasMaxLength(50);

        // TODO: Cryptos require at least 4 characters!
        builder.Property(e => e.CurrencyCode)
            .HasMaxLength(3);

        builder.Property(e => e.Tags)
            .HasMaxLength(150);

        builder.HasIndex(e => e.Name)
            .IsUnique();

        builder.UseTphMappingStrategy();
    }
}
