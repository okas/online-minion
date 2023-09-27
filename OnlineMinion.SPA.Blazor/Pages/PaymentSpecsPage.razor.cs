using JetBrains.Annotations;
using OnlineMinion.Contracts.CurrencyInfo.Requests;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.SPA.Blazor.Components;
using OnlineMinion.SPA.Blazor.CurrencyInfo.ViewModels;
using OnlineMinion.SPA.Blazor.Pages.Base;

namespace OnlineMinion.SPA.Blazor.Pages;

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

    protected override CreatePaymentSpecReq CreateVMFactory() => new();

    protected override UpdatePaymentSpecReq UpdateVMFactory(PaymentSpecResp vm) =>
        new(vm.Id, vm.Name, vm.CurrencyCode, vm.Tags);

    protected override PaymentSpecResp ConvertReqResponseToVM(PaymentSpecResp dto) => dto;

    protected override IUpdateCommand ConvertUpdateVMToReq(IUpdateCommand reqOrVM) => reqOrVM;

    protected override PaymentSpecResp ConvertUpdateReqToVM(IUpdateCommand dto) =>
        (PaymentSpecResp)(UpdatePaymentSpecReq)dto;

    protected override ICreateCommand ConvertCreateVMToReq(ICreateCommand reqOrVM) => reqOrVM;

    protected override IGetPagingInfoRequest PageCountRequestFactory(int pageSize) =>
        new GetPaymentSpecPagingMetaInfoReq(pageSize);

    protected override string GetDeleteMessageDescriptorData(PaymentSpecResp model) => model.Name;

    protected override DeletePaymentSpecReq DeleteCommandFactory(PaymentSpecResp vm) => new(vm.Id);
}
