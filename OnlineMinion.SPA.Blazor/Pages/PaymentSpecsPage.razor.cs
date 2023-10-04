using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.CurrencyInfo.Requests;
using OnlineMinion.Application.Contracts.PaymentSpecCash.Requests;
using OnlineMinion.Application.Contracts.PaymentSpecShared;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Responses;
using OnlineMinion.Application.Contracts.Shared.Requests;
using OnlineMinion.SPA.Blazor.Components;
using OnlineMinion.SPA.Blazor.CurrencyInfo.ViewModels;
using OnlineMinion.SPA.Blazor.Pages.Base;
using OnlineMinion.SPA.Blazor.ViewModels;
using OnlineMinion.SPA.Blazor.ViewModels.PaymentSpec;

namespace OnlineMinion.SPA.Blazor.Pages;

[UsedImplicitly]
public partial class PaymentSpecsPage : BaseCRUDPage<PaymentSpecResp, PaymentSpecResp, BaseUpsertPaymentSpecReqData>
{
    private readonly List<CurrencyInfoVm> _currencyCodes = new();

    private PaymentSpecCashEditor? _editorRef;

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

    protected override CreatePaymentSpecCashReq CreateVMFactory() => new();

    protected override UpdatePaymentSpecCashReq UpdateVMFactory(PaymentSpecResp vm) =>
        new(vm.Id, vm.Name, vm.Tags);

    protected override PaymentSpecResp ConvertReqResponseToVM(PaymentSpecResp dto) => dto;

    protected override IUpdateCommand ConvertUpdateVMToReq(IUpdateCommand reqOrVM) => reqOrVM;

    protected override PaymentSpecResp ConvertUpdateReqToVM(IUpdateCommand dto, PaymentSpecResp vm)
    {
        var updateReq = (UpdatePaymentSpecCashReq)dto;

        return vm with { Name = updateReq.Name, Tags = updateReq.Tags, };
    }

    protected override ICreateCommand ConvertCreateVMToReq(ICreateCommand reqOrVM) => reqOrVM;

    protected override IGetPagingInfoRequest PageCountRequestFactory(int pageSize) =>
        new GetPaymentSpecPagingMetaInfoReq(pageSize);

    protected override string GetDeleteDialogMessage(PaymentSpecResp vm) => GetDeleteDialogMessageFormat.Replace(
        "{}",
        $"'{GetTypeName(vm.Type)} {ModelTypeName}' with name <em>{vm.Name}</em>"
    );

    protected override DeletePaymentSpecReq DeleteCommandFactory(PaymentSpecResp vm) => new(vm.Id);

    protected override IEditorMetadata<PaymentSpecResp> EditorMetadataFactory(PaymentSpecResp vm) =>
        new UpdatePaymentSpecEditorMetadata(vm);

    private static string GetTypeName(PaymentSpecType type) =>
        type switch
        {
            PaymentSpecType.Cash => nameof(PaymentSpecType.Cash),
            PaymentSpecType.Bank => nameof(PaymentSpecType.Bank),
            PaymentSpecType.Crypto => nameof(PaymentSpecType.Crypto),
            _ => throw new ArgumentOutOfRangeException(
                nameof(type),
                type,
                "Payment Spec type is not supported"
            ),
        };
}
