using FluentValidation;

namespace OnlineMinion.RestApi.Validation;

/// <summary>
///     A marker interface for validators that validate uniqueness of a property asynchronously.
/// </summary>
/// <remarks>
///     It needs to applied on specific validators of models <typeparamref name="TModel" />, not for their base
///     classes, because of the way MS DI resolves service implementations.
/// </remarks>
/// <typeparam name="TModel">Model to validate.</typeparam>
public interface IAsyncUniqueValidator<in TModel> : IValidator<TModel> { }
