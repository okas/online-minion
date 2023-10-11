using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.DataStore.EntityConfiguration;

public class BasePaymentSpecEntityConfig : IEntityTypeConfiguration<BasePaymentSpecData>
{
    public void Configure(EntityTypeBuilder<BasePaymentSpecData> builder)
    {
        builder.UseTphMappingStrategy()
            .ToTable("PaymentSpecs")
            .HasDiscriminator<string>("Discriminator")
            .HasValue<PaymentSpecCash>("Cash")
            .HasValue<PaymentSpecBank>("Bank")
            .HasValue<PaymentSpecCrypto>("Crypto");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasMaxLength(50);

        // TODO: Cryptos require at least 4 characters!
        builder.Property(e => e.CurrencyCode)
            .HasMaxLength(3);

        builder.Property(e => e.Tags)
            .HasMaxLength(150);

        builder.HasIndex(e => e.Name)
            .IsUnique();
    }
}
