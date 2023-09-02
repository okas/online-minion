using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Web.Pages.Base;

namespace OnlineMinion.Web.Pages;

[UsedImplicitly]
public partial class AccountSpecsPage : BaseCRUDPage<AccountSpecResp, AccountSpecResp, BaseUpsertAccountSpecReqData>
{
    protected override string ModelTypeName => "Account Specification";

    protected override ICreateCommand CreateCommandFactory() => new CreateAccountSpecReq();

    protected override IUpdateCommand UpdateCommandFactory(AccountSpecResp vm) =>
        new UpdateAccountSpecReq(vm.Id, vm.Name, vm.Group, vm.Description);

    protected override AccountSpecResp ConvertReqResponseToVM(AccountSpecResp dto) => dto;

    protected override AccountSpecResp ConvertUpdateReqToVM(IUpdateCommand dto) =>
        (AccountSpecResp)(UpdateAccountSpecReq)dto;

    protected override IGetPagingInfoRequest PageCountRequestFactory(int pageSize) =>
        new GetAccountPagingMetaInfoReq(pageSize);

    protected override string GetDeleteMessageDescriptorData(AccountSpecResp model) => model.Name;

    protected override IDeleteByIdCommand DeleteCommandFactory(AccountSpecResp vm) => new DeleteAccountSpecReq(vm.Id);
}
