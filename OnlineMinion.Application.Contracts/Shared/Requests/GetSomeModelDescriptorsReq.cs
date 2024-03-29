namespace OnlineMinion.Application.Contracts.Shared.Requests;

public record GetSomeModelDescriptorsReq<TResponse> : IGetStreamedRequest<TResponse>
    where TResponse : IHasId;
