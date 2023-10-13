using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.DataStore.EntityConfiguration;

public class PaymentSpecBankEntityConfig : IEntityTypeConfiguration<PaymentSpecBank>
{
    public void Configure(EntityTypeBuilder<PaymentSpecBank> builder)
    {
        builder.Property(e => e.BankName)
            .HasMaxLength(75);

        builder.Property(e => e.IBAN)
            .HasMaxLength(34);

        builder.HasIndex(e => e.IBAN)
            .IsUnique();

        builder.ToTable(
            tb => tb.HasCheckConstraint(
                "CK_PaymentSpecBank_IBAN_Length",
                "LEN(IBAN) BETWEEN 16 AND 34"
            )
        );
    }
}
