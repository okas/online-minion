namespace OnlineMinion.SPA.Blazor.ViewModels;

/// <summary>
///     Provides strongly typed metadata object contract. Implementation should have 1* fields of type
///     <typeparamref name="TViewModel" />'s.
/// </summary>
/// <remarks>
///     Use members of type <see cref="FiledMetadata{TValue}" />, this is strongly typed interface for metadata itself.
/// </remarks>
/// <typeparam name="TViewModel">
///     Viewmodel's type that that provides reasonable base for metadata, normally viewmodel of the rendered page, but it
///     is not restricted to it as long as UI can suck out expected metadata and optionally model values for a field.
/// </typeparam>
/// <example>
///     This is example implementation with one field metadata definition.
///     <code>
///     public readonly record struct UpdatePaymentSpecEditorMetadata : IEditorMetadata&lt;PaymentSpecResp&gt;
///     {
///         public UpdatePaymentSpecEditorMetadata(PaymentSpecResp viewModel) =&gt; CurrencyCode = new(viewModel.CurrencyCode, true);
///         public FiledMetadata&lt;string&gt; CurrencyCode { get; }
///     }
///     </code>
/// </example>
public interface IEditorMetadata<TViewModel>;
