namespace OnlineMinion.Application.Contracts;

public interface IQueryParameters : IPagingInfo
{
    /// <summary>
    ///     Filter string <a href="https://dynamic-linq.net/basic-simple-query#more-where-examples">see docs</a>.
    /// </summary>
    string? Filter { get; }

    /// <summary>
    ///     Sort/OrderBy string <a href="https://dynamic-linq.net/basic-simple-query#ordering-results">see docs</a>.
    /// </summary>
    string? Sort { get; }
}
