using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.DataStore.EntityConfiguration;

public class CryptoAccountEntityConfig : IEntityTypeConfiguration<PaymentSpecCrypto>
{
    public void Configure(EntityTypeBuilder<PaymentSpecCrypto> builder)
    {
        builder.Property(e => e.ExchangeName)
            .HasMaxLength(75);

        builder.Property(e => e.IsFiat);
    }
}
