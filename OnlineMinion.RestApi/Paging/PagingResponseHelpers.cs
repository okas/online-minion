using System.Globalization;
using Microsoft.AspNetCore.Http;
using OnlineMinion.Contracts;

namespace OnlineMinion.RestApi.Paging;

public static class PagingResponseHelpers
{
    /// <summary>
    ///     Sets response headers of paging related data.
    /// </summary>
    /// <param name="response"></param>
    /// <param name="values">Paging meta info to set into response headers.</param>
    public static void SetPagingHeaders(this HttpResponse response, PagingMetaInfo values)
    {
        var headers = response.Headers;

        headers[CustomHeaderNames.PagingRows] = values.Rows.ToString(NumberFormatInfo.InvariantInfo);
        headers[CustomHeaderNames.PagingSize] = values.Size.ToString(NumberFormatInfo.InvariantInfo);
        headers[CustomHeaderNames.PagingPages] = values.Pages.ToString(NumberFormatInfo.InvariantInfo);
    }
}
