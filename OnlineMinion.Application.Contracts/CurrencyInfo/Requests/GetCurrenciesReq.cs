using OnlineMinion.Application.Contracts.CurrencyInfo.Responses;
using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.CurrencyInfo.Requests;

public record GetCurrenciesReq : IGetStreamedRequest<CurrencyInfoResp>;
