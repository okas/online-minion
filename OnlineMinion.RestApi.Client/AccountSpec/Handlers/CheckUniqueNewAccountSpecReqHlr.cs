using System.Globalization;
using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.RestApi.Client.Common.Handlers;
using OnlineMinion.RestApi.Client.Infrastructure;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class CheckUniqueNewAccountSpecReqHlr : BaseCheckUniqueReqHlr<CheckAccountSpecUniqueNewReq>
{
    public CheckUniqueNewAccountSpecReqHlr(ApiClientProvider api) : base(api) { }

    protected override Uri BuildUrl(CheckAccountSpecUniqueNewReq rq) => new(
        string.Create(
            CultureInfo.InvariantCulture,
            $"{Api.ApiV1AccountSpecsUri}/validate-available-name/{rq.Name}"
        ),
        UriKind.RelativeOrAbsolute
    );
}
