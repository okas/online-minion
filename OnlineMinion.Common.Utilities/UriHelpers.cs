using JetBrains.Annotations;
using Microsoft.AspNetCore.WebUtilities;
using static System.String;

namespace OnlineMinion.Common.Utilities;

// TODO: needs fixes and optimizations, see MA analyzer result for input! Also,  QueryHelpers.AddQueryString
// is already taking care of null values, so we can remove that check.

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
                    Join(",", kvp.Value.Select(v => IsNullOrWhiteSpace(v?.ToString()) ? "" : v))
                )
            ),
            StringComparer.OrdinalIgnoreCase
        );

        return QueryHelpers.AddQueryString(uri, queryStringParameters!);
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
                        (IsNullOrWhiteSpace(kvp.Value?.ToString())
                            ? Empty
                            : kvp.Value?.ToString())
                        ?? Empty
                    )
            ),
            StringComparer.OrdinalIgnoreCase
        );

        return QueryHelpers.AddQueryString(uri, queryStringParameters!);
    }
}
