namespace OnlineMinion.Application.Contracts.PaymentSpecBank;

public interface IUpsertPaymentSpecBankReq
{
    string IBAN { get; set; }

    string BankName { get; set; } // TODO: to be removed
}
