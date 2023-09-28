using Microsoft.EntityFrameworkCore;
using OnlineMinion.Domain;
using OnlineMinion.Domain.Shared;

namespace OnlineMinion.Application;

public interface IOnlineMinionDbContext
{
    DbSet<AccountSpec> AccountSpecs { get; set; }

    DbSet<TransactionDebit> TransactionDebits { get; set; }

    DbSet<TransactionCredit> TransactionCredits { get; set; }

    DbSet<BasePaymentSpec> PaymentSpecs { get; set; }

    DbSet<BankAccountSpec> BankAccountSpecs { get; set; }

    DbSet<CashAccountSpec> CashAccountsSpecs { get; set; }

    DbSet<CryptoExchangeAccountSpec> CryptoExchangeAccountSpecs { get; set; }

    Task<int> SaveChangesAsync() => SaveChangesAsync(default);

    DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;

    Task<int> SaveChangesAsync(CancellationToken ct);
}
