using System.Net;
using System.Net.Http.Json;
using FluentResults;
using MediatR;
using OnlineMinion.Common.Errors;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.RestApi.Client.Infrastructure;

namespace OnlineMinion.RestApi.Client.Handlers;

internal sealed class UpdateAccountSpecReqHlr : IRequestHandler<UpdateAccountSpecReq, Result>
{
    private readonly ApiClientProvider _api;
    public UpdateAccountSpecReqHlr(ApiClientProvider api) => _api = api;

    public async Task<Result> Handle(UpdateAccountSpecReq request, CancellationToken cancellationToken)
    {
        var uri = $"{_api.ApiV1AccountSpecsUri}/{request.Id}";
        using var message = await _api.Client.PutAsJsonAsync(uri, request, cancellationToken).ConfigureAwait(false);

        if (message.IsSuccessStatusCode)
        {
            return Result.Ok();
        }

        var result = message.StatusCode == HttpStatusCode.NotFound
            ? Result.Fail(new NotFoundError(message.Content.ToString()))
            : Result.Fail(message.ReasonPhrase);

        result.Log<UpdateAccountSpecReqHlr>();

        return result;
    }
}
