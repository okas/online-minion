using Microsoft.OpenApi.Models;
using OnlineMinion.Application.Contracts;

namespace OnlineMinion.RestApi.Paging;

public static class DocumentationHelpers
{
    /// <summary>Adds paging meta info headers to the operation.</summary>
    /// <remarks>
    ///     If response with provided <paramref name="statusCode" /> do not exist, new response will be created.
    /// </remarks>
    /// <param name="operation"></param>
    /// <param name="statusCode"></param>
    /// <returns></returns>
    public static OpenApiOperation SetPagingMetaInfoHeaders(this OpenApiOperation operation, int statusCode)
    {
        var responseKey = statusCode.ToString();
        if (!operation.Responses.TryGetValue(statusCode.ToString(), out var response))
        {
            response = operation.Responses[responseKey] = new();
        }

        response.Headers[CustomHeaderNames.PagingSize] = new()
        {
            Description = Str.SizeDescription, Required = true,
            Schema = new() { Type = "integer", Minimum = 1, Maximum = 100, },
        };

        response.Headers[CustomHeaderNames.PagingRows] = new()
        {
            Description = Str.RowsDescription, Required = true, Schema = new() { Type = "integer", Minimum = 0, },
        };

        response.Headers[CustomHeaderNames.PagingPages] = new()
        {
            Description = Str.PagesDescription, Required = true, Schema = new() { Type = "integer", Minimum = 0, },
        };

        return operation;
    }

    private static class Str
    {
        public const string RowsDescription = "Total items of resource.";
        public const string SizeDescription = "Page size used.";
        public const string PagesDescription = "Pages, based on provided page size.";
    }
}
