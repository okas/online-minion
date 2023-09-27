using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.SPA.Blazor.Components;
using OnlineMinion.SPA.Blazor.Pages.Base;

namespace OnlineMinion.SPA.Blazor.Pages;

[UsedImplicitly]
public partial class AccountSpecsPage : BaseCRUDPage<AccountSpecResp, AccountSpecResp, BaseUpsertAccountSpecReqData>
{
    private AccountSpecsEditor? _editorRef;

    protected override string ModelTypeName => "Account Specification";

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

    protected override CreateAccountSpecReq CreateVMFactory() => new();

    protected override UpdateAccountSpecReq UpdateVMFactory(AccountSpecResp vm) =>
        new(vm.Id, vm.Name, vm.Group, vm.Description);

    protected override AccountSpecResp ConvertReqResponseToVM(AccountSpecResp dto) => dto;

    protected override IUpdateCommand ConvertUpdateVMToReq(IUpdateCommand reqOrVM) => reqOrVM;

    protected override AccountSpecResp ConvertUpdateReqToVM(IUpdateCommand dto) =>
        (AccountSpecResp)(UpdateAccountSpecReq)dto;

    protected override ICreateCommand ConvertCreateVMToReq(ICreateCommand reqOrVM) => reqOrVM;

    protected override IGetPagingInfoRequest PageCountRequestFactory(int pageSize) =>
        new GetAccountSpecPagingMetaInfoReq(pageSize);

    protected override string GetDeleteMessageDescriptorData(AccountSpecResp model) => model.Name;

    protected override DeleteAccountSpecReq DeleteCommandFactory(AccountSpecResp vm) => new(vm.Id);
}
