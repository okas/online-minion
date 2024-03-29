using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineMinion.Domain;

namespace OnlineMinion.DataStore.EntityConfiguration;

public class BankAccountEntityConfig : IEntityTypeConfiguration<BankAccountSpec>
{
    public void Configure(EntityTypeBuilder<BankAccountSpec> builder)
    {
        builder.Property(e => e.BankName)
            .HasMaxLength(75);

        builder.Property(e => e.IBAN)
            .HasMaxLength(34);
    }
}
