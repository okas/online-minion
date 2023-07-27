using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineMinion.Data.Entities;

namespace OnlineMinion.Data.EntityConfiguration;

public class CryptoAccountEntityConfig : IEntityTypeConfiguration<CryptoAccountSpec>
{
    public void Configure(EntityTypeBuilder<CryptoAccountSpec> builder)
    {
        builder.Property(e => e.ExchangeName)
            .HasMaxLength(75);
    }
}
