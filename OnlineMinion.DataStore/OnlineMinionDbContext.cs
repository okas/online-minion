using EntityFramework.Exceptions.Common;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Application;
using OnlineMinion.Application.Exceptions;
using OnlineMinion.DataStore.Helpers;
using OnlineMinion.Domain.AccountSpecs;
using OnlineMinion.Domain.PaymentSpecs;
using OnlineMinion.Domain.TransactionCredits;
using OnlineMinion.Domain.TransactionDebits;

namespace OnlineMinion.DataStore;

public class OnlineMinionDbContext(DbContextOptions<OnlineMinionDbContext> options)
    : DbContext(options), IOnlineMinionDbContext
{
    public DbSet<AccountSpec> AccountSpecs { get; set; } = null!;

    public DbSet<TransactionDebit> TransactionDebits { get; set; } = null!;

    public DbSet<TransactionCredit> TransactionCredits { get; set; } = null!;

    public DbSet<BankAccountSpec> BankAccountSpecs { get; set; } = null!;

    public DbSet<CashAccountSpec> CashAccountsSpecs { get; set; } = null!;

    public DbSet<CryptoExchangeAccountSpec> CryptoExchangeAccountSpecs { get; set; } = null!;

    DbSet<TEntity> IOnlineMinionDbContext.Set<TEntity>() => base.Set<TEntity>();

    /// <inheritdoc cref="DbContext" />
    public new Task<int> SaveChangesAsync(CancellationToken ct = default) => SaveChangesAsync(true, ct);

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
            throw new ConflictException(Str.MsgUniqueViolation, ex);
        }
    }

    /// <inheritdoc cref="DbContext" />
    public new async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken ct = default)
    {
        try
        {
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, ct).ConfigureAwait(false);
        }
        catch (UniqueConstraintException ex)
        {
            throw new ConflictException(Str.MsgUniqueViolation, ex);
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseExceptionProcessor();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) =>
        configurationBuilder.ApplyValueConversionsFromAssembly(typeof(OnlineMinionDbContext).Assembly);

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OnlineMinionDbContext).Assembly);

    private static class Str
    {
        public const string MsgUniqueViolation = "Field(s) must have unique values";
    }
}
