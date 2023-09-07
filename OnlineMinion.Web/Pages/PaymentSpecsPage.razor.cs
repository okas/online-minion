using JetBrains.Annotations;
using OnlineMinion.Contracts.CurrencyInfo.Requests;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Web.Components;
using OnlineMinion.Web.Pages.Base;
using OnlineMinion.Web.ViewModels.CurrencyInfo;

namespace OnlineMinion.Web.Pages;

[UsedImplicitly]
public partial class PaymentSpecsPage : BaseCRUDPage<PaymentSpecResp, PaymentSpecResp, BaseUpsertPaymentSpecReqData>
{
    private readonly List<CurrencyInfoVm> _currencyCodes = new();

    private PaymentSpecsEditor? _editorRef;

    protected override string ModelTypeName => "Payment Specification";

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        // In this page's case, the editor is separate component that encapsulates own content inside EditorWrapper.
        // Therefor, exposed parameter WrapperRef is only accessible if editor itself is rendered
        // and it can be pulled out now.
        if (_editorRef is not null)
        {
            EditorWrapperRef = _editorRef.WrapperRef;
        }
    }

    protected override async Task RunDependencyLoadingAsync() => await LoadDependencyFromApiAsync(
        new GetCurrenciesReq(),
        resp => _currencyCodes.Add(new(resp))
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
