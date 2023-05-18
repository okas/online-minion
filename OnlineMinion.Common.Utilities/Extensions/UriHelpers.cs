using JetBrains.Annotations;
using Microsoft.AspNetCore.WebUtilities;

namespace OnlineMinion.Common.Utilities.Extensions;

public static class UriHelpers
{
    public static string AddQueryString(this Uri uri, IReadOnlyDictionary<string, IEnumerable<object?>> parameters) =>
        uri.ToString().AddQueryString(parameters);

    public static string AddQueryString(
        [UriString] this string                           uri,
        IReadOnlyDictionary<string, IEnumerable<object?>> parameters
    )
    {
        var queryStringParameters = new Dictionary<string, string>(
            parameters.Select(
                kvp => new KeyValuePair<string, string>(
                    kvp.Key,
                    string.Join(",", kvp.Value.Select(v => string.IsNullOrWhiteSpace(v?.ToString()) ? "" : v))
                )
            )
        );

        return QueryHelpers.AddQueryString(uri, queryStringParameters);
    }

    public static string AddQueryString(this Uri uri, IReadOnlyDictionary<string, object?> parameters) =>
        uri.ToString().AddQueryString(parameters);

    public static string AddQueryString([UriString] this string uri, IReadOnlyDictionary<string, object?> parameters)
    {
        var queryStringParameters = new Dictionary<string, string>(
            parameters.Select(
                kvp =>
                    new KeyValuePair<string, string>(
                        kvp.Key,
                        (string.IsNullOrWhiteSpace(kvp.Value?.ToString())
                            ? string.Empty
                            : kvp.Value?.ToString())
                        ?? string.Empty
                    )
            )
        );

        return QueryHelpers.AddQueryString(uri, queryStringParameters);
    }
}
