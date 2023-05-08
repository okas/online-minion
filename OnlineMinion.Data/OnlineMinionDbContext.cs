using Microsoft.EntityFrameworkCore;
using OnlineMinion.Data.BaseEntities;
using OnlineMinion.Data.Entities;
using OnlineMinion.Data.EntityConfiguration;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        new AccountSpecEntityConfig().Configure(modelBuilder.Entity<AccountSpec>());
        new TransactionDebitEntityConfig().Configure(modelBuilder.Entity<TransactionDebit>());
        new BasePaymentSpecEntityConfig().Configure(modelBuilder.Entity<BasePaymentSpec>());
        new BankAccountEntityConfig().Configure(modelBuilder.Entity<BankAccountSpec>());
        new BaseTransactionEntityConfig().Configure(modelBuilder.Entity<BaseTransaction>());
    }
}
