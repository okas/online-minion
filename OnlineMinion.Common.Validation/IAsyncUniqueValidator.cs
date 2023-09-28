using FluentValidation;

namespace OnlineMinion.Common.Validation;

/// <summary>
///     A marker interface for validators that validate uniqueness of a property asynchronously.
/// </summary>
/// <typeparam name="TModel">Model to validate.</typeparam>
public interface IAsyncUniqueValidator<in TModel> : IValidator<TModel>;
