using Microsoft.AspNetCore.Http;
using OnlineMinion.Contracts;
using static OnlineMinion.Contracts.CustomHeaderNames;

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
        response.Headers[PagingRows] = values.Rows.ToString();
        response.Headers[PagingSize] = values.Size.ToString();
        response.Headers[PagingPages] = values.Pages.ToString();
    }
}
