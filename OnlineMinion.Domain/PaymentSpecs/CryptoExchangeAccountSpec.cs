namespace OnlineMinion.Domain.PaymentSpecs;

public sealed class CryptoExchangeAccountSpec : BasePaymentSpecData
{
    // ReSharper disable once UnusedMember.Local
    private CryptoExchangeAccountSpec() { }

    public CryptoExchangeAccountSpec(string exchangeName, bool isFiat, string name, string currencyCode, string? tags)
        : base(name, currencyCode, tags) => (ExchangeName, IsFiat) = (exchangeName, isFiat);

    public string ExchangeName { get; private set; } = null!;

    public bool IsFiat { get; private set; }

    public void Update(string exchangeName, bool isFiat, string name, string? tags)
    {
        base.Update(name, tags);
        ExchangeName = exchangeName;
        IsFiat = isFiat;
    }
}
