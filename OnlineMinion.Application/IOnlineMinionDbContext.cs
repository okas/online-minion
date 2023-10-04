using Microsoft.EntityFrameworkCore;
using OnlineMinion.Domain;
using OnlineMinion.Domain.PaymentSpecs;
using OnlineMinion.Domain.TransactionDebits;

namespace OnlineMinion.Application;

public interface IOnlineMinionDbContext
{
    DbSet<Domain.AccountSpecs.AccountSpec> AccountSpecs { get; set; }

    DbSet<TransactionDebit> TransactionDebits { get; set; }

    DbSet<Domain.TransactionCredits.TransactionCredit> TransactionCredits { get; set; }

    DbSet<PaymentSpecBank> BankAccountSpecs { get; set; }

    DbSet<PaymentSpecCash> CashAccountsSpecs { get; set; }

    DbSet<CryptoExchangeAccountSpec> CryptoExchangeAccountSpecs { get; set; }

    Task<int> SaveChangesAsync() => SaveChangesAsync(default);

    DbSet<TEntity> Set<TEntity>()
        where TEntity : class, IEntity<IId>;

    Task<int> SaveChangesAsync(CancellationToken ct);
}
