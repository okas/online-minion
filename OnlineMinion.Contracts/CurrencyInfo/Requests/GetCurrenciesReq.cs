using OnlineMinion.Contracts.CurrencyInfo.Responses;
using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.Contracts.CurrencyInfo.Requests;

public record GetCurrenciesReq : IGetStreamedRequest<CurrencyInfoResp>;
