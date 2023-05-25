using System.Net;
using System.Net.Http.Json;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.RestApi.Client.Infrastructure;

namespace OnlineMinion.RestApi.Client.Handlers;

internal sealed class CreateAccountSpecReqHlr : IRequestHandler<CreateAccountSpecReq, Result<ModelIdResp>>
{
    private readonly ApiClientProvider _api;

    public CreateAccountSpecReqHlr(ApiClientProvider api) => _api = api;

    public async Task<Result<ModelIdResp>> Handle(
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
                return modelIdResp;
            }

            throw CreateException(nameof(ModelIdResp));
        }

        if (message.StatusCode == HttpStatusCode.Conflict)
        {
            if (await message.Content.ReadFromJsonAsync<HttpValidationProblemDetails?>(cancellationToken)
                    .ConfigureAwait(false) is { } validationError)
            {
                return new Error(validationError.Detail).WithMetadata(
                    validationError.Errors.ToDictionary(
                        kvp => kvp.Key,
                        kvp => (object)kvp.Value,
                        StringComparer.Ordinal
                    )
                );
            }

            throw CreateException(nameof(HttpValidationProblemDetails));
        }

        if (await message.Content.ReadFromJsonAsync<ProblemDetails?>(cancellationToken)
                .ConfigureAwait(false) is { } problem)
        {
            return new Error(problem.Detail).WithMetadata(
                problem.Extensions.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value ?? string.Empty,
                    StringComparer.Ordinal
                )
            );
        }

        throw CreateException(nameof(ProblemDetails));
    }

    private static InvalidOperationException CreateException(string name) =>
        new($"Got unknown response from API, expected type of `{name}`.");
}
