using EntityFramework.Exceptions.Common;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Data.Entities;
using OnlineMinion.Data.Entities.Shared;
using OnlineMinion.Data.Exceptions;

namespace OnlineMinion.Data;

public class OnlineMinionDbContext(DbContextOptions<OnlineMinionDbContext> options) : DbContext(options)
{
    public DbSet<AccountSpec> AccountSpecs { get; set; } = null!;

    public DbSet<TransactionDebit> TransactionDebits { get; set; } = null!;

    public DbSet<TransactionCredit> TransactionCredits { get; set; } = null!;

    public DbSet<BasePaymentSpec> PaymentSpecs { get; set; } = null!;

    public DbSet<BankAccountSpec> BankAccountSpecs { get; set; } = null!;

    public DbSet<CashAccountSpec> CashAccountsSpecs { get; set; } = null!;

    public DbSet<CryptoExchangeAccountSpec> CryptoExchangeAccountSpecs { get; set; } = null!;

    /// <inheritdoc cref="DbContext" />
    public new int SaveChanges() => SaveChanges(true);

    /// <inheritdoc cref="DbContext" />
    public new int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        try
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
        catch (UniqueConstraintException ex)
        {
            throw new ConflictException("Field(s) must have unique values", ex);
        }
    }

    /// <inheritdoc cref="DbContext" />
    public new Task<int> SaveChangesAsync(CancellationToken ct = default) => SaveChangesAsync(true, ct);

    /// <inheritdoc cref="DbContext" />
    public new async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken ct = default)
    {
        try
        {
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, ct).ConfigureAwait(false);
        }
        catch (UniqueConstraintException ex)
        {
            throw new ConflictException("Field(s) must have unique values", ex);
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseExceptionProcessor();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseHiLo();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OnlineMinionDbContext).Assembly);
    }
}
