using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.DataStore.EntityConfiguration;

public class BankAccountSpecEntityConfig : IEntityTypeConfiguration<BankAccountSpec>
{
    public void Configure(EntityTypeBuilder<BankAccountSpec> builder)
    {
        builder.Property(e => e.BankName)
            .HasMaxLength(75);

        builder.Property(e => e.IBAN)
            .HasMaxLength(34);
    }
}
