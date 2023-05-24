using System.Net;
using System.Net.Http.Json;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineMinion.Contracts.AppMessaging;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.RestApi.Client.Infrastructure;

namespace OnlineMinion.RestApi.Client.Handlers;

internal sealed class CreateAccountSpecReqHlr : IRequestHandler<CreateAccountSpecReq, HandlerResult<ModelIdResp>>
{
    private readonly ApiClientProvider _api;

    public CreateAccountSpecReqHlr(ApiClientProvider api) => _api = api;

    public async Task<HandlerResult<ModelIdResp>> Handle(
        CreateAccountSpecReq request,
        CancellationToken    cancellationToken
    )
    {
        using var message = await _api.Client.PostAsJsonAsync(_api.ApiV1AccountSpecsUri, request, cancellationToken)
            .ConfigureAwait(false);

        if (message.IsSuccessStatusCode)
        {
            if (await message.Content.ReadFromJsonAsync<ModelIdResp>(cancellationToken).ConfigureAwait(false)
                is { Id: > 0, } modelIdResp)
            {
                return HandlerResult<ModelIdResp>.Success(modelIdResp);
            }

            throw new InvalidOperationException("Unknown response, expected type of `ModelIdResp`.");
        }

        if (message.StatusCode == HttpStatusCode.Conflict
            && await message.Content.ReadFromJsonAsync<HttpValidationProblemDetails?>(cancellationToken)
                .ConfigureAwait(false) is { } validationError)
        {
            return HandlerResult<ModelIdResp>.Failure(validationError!.Detail!, validationError.Errors);
        }

        var error = await message.Content.ReadFromJsonAsync<ProblemDetails?>(cancellationToken)
            .ConfigureAwait(false);

        return HandlerResult<ModelIdResp>.Failure(error?.Detail, error.Extensions);
    }
}
