using JetBrains.Annotations;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Web.Pages.Base;

namespace OnlineMinion.Web.Pages;

[UsedImplicitly]
public partial class PaymentSpecsPage : BaseCRUDPage<PaymentSpecResp, PaymentSpecResp, BaseUpsertPaymentSpecReqData>
{
    private readonly List<CurrencyInfoResp> _currencyCodes = new();

    protected override string ModelTypeName => "Payment Specification";

    protected override Task LoadDependenciesAsync() => LoadDependentVMsFromApiAsync(
        new GetCurrenciesReq(),
        _currencyCodes
    );

    protected override ICreateCommand CreateCommandFactory() => new CreatePaymentSpecReq();

    protected override IUpdateCommand UpdateCommandFactory(PaymentSpecResp vm) =>
        new UpdatePaymentSpecReq(vm.Id, vm.Name, vm.CurrencyCode, vm.Tags);

    protected override PaymentSpecResp ConvertReqResponseToVM(PaymentSpecResp dto) => dto;

    protected override PaymentSpecResp ConvertUpdateReqToVM(IUpdateCommand dto) =>
        (PaymentSpecResp)(UpdatePaymentSpecReq)dto;

    protected override IGetPagingInfoRequest PageCountRequestFactory(int pageSize) =>
        new GetPaymentSpecPagingMetaInfoReq(pageSize);

    protected override string GetDeleteMessageDescriptorData(PaymentSpecResp model) => model.Name;

    protected override IDeleteByIdCommand DeleteCommandFactory(PaymentSpecResp vm) => new DeletePaymentSpecReq(vm.Id);
}
