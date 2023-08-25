using System.ComponentModel;
using System.Net.Http.Headers;

namespace OnlineMinion.Common.Utilities;

public static class HttpResponseHeadersExtensions
{
    public static TVal? GetHeaderFirstValue<TVal>(this HttpResponseHeaders headers, string headerName)
    {
        if (!headers.TryGetValues(headerName, out var vals) || vals.ToArray() is not [var val,])
        {
            return default;
        }

        var converter = TypeDescriptor.GetConverter(typeof(TVal));

        return converter.CanConvertFrom(typeof(string))
            ? (TVal?)converter.ConvertFromInvariantString(val)
            : default;
    }
}
