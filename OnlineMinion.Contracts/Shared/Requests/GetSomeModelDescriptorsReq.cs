namespace OnlineMinion.Contracts.Shared.Requests;

public record GetSomeModelDescriptorsReq<TResponse> : IGetStreamedRequest<TResponse>
    where TResponse : IHasIntId;
