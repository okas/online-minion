namespace OnlineMinion.Web.CQRS.QueryHandlers;

internal class BaseAccountSpecsRequestHandler
{
    protected static readonly Uri UriApiV1AccountSpecs = new("api/v1/AccountSpecs", UriKind.Relative);
}