using EntityFramework.Exceptions.Common;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Data.BaseEntities;
using OnlineMinion.Data.Entities;
using OnlineMinion.Data.Exceptions;

namespace OnlineMinion.Data;

public class OnlineMinionDbContext : DbContext
{
    public OnlineMinionDbContext(DbContextOptions<OnlineMinionDbContext> options) : base(options) { }

    public DbSet<AccountSpec> AccountSpecs { get; set; } = null!;

    public DbSet<TransactionDebit> TransactionDebits { get; set; } = null!;

    public DbSet<TransactionCredit> TransactionCredits { get; set; } = null!;

    public DbSet<BasePaymentSpec> PaymentSpecs { get; set; } = null!;

    public DbSet<BankAccountSpec> BankAccountSpecs { get; set; } = null!;

    public DbSet<CashAccountSpec> CashAccountsSpecs { get; set; } = null!;

    public new int SaveChanges() => SaveChanges(true);

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
            return await base.SaveChangesAsync(ct).ConfigureAwait(false);
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
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OnlineMinionDbContext).Assembly);
    }
}
