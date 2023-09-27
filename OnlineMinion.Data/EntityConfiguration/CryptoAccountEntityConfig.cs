using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineMinion.Domain;

namespace OnlineMinion.Data.EntityConfiguration;

public class CryptoAccountEntityConfig : IEntityTypeConfiguration<CryptoExchangeAccountSpec>
{
    public void Configure(EntityTypeBuilder<CryptoExchangeAccountSpec> builder)
    {
        builder.Property(e => e.ExchangeName)
            .HasMaxLength(75);
    }
}
