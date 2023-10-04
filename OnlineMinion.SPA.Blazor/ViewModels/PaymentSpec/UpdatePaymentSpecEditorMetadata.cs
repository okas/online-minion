using OnlineMinion.Application.Contracts.PaymentSpecShared.Responses;

namespace OnlineMinion.SPA.Blazor.ViewModels.PaymentSpec;

/// <summary>Following is implemented interface's documentation:</summary>
/// <inheritdoc cref="IEditorMetadata{TViewModel}" />
public readonly record struct UpdatePaymentSpecEditorMetadata : IEditorMetadata<PaymentSpecResp>
{
    public UpdatePaymentSpecEditorMetadata(PaymentSpecResp viewModel) =>
        CurrencyCode = new(viewModel.CurrencyCode, true);

    public FiledMetadata<string?> CurrencyCode { get; }
}
