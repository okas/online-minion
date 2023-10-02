using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineMinion.Domain.AccountSpecs;

namespace OnlineMinion.DataStore.EntityConfiguration;

public class AccountSpecEntityConfig : IEntityTypeConfiguration<AccountSpec>
{
    public void Configure(EntityTypeBuilder<AccountSpec> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasMaxLength(50);

        builder.Property(e => e.Group)
            .HasMaxLength(50);

        builder.Property(e => e.Description)
            .HasMaxLength(150);

        builder.HasIndex(e => e.Name)
            .IsUnique();
    }
}
