namespace OnlineMinion.RestApi.Client;

public static class ExceptionHelpers
{
    public static InvalidOperationException CreateForUnknownResponse(string name) =>
        new($"Got unknown response from API, expected type of `{name}`.");
}
