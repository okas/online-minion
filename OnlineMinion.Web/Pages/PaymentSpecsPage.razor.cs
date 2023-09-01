using ErrorOr;
using JetBrains.Annotations;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Web.Components;
using OnlineMinion.Web.Pages.Base;

namespace OnlineMinion.Web.Pages;

[UsedImplicitly]
public partial class PaymentSpecsPage : BaseCRUDPage<PaymentSpecResp, PaymentSpecResp>
{
    private const string ModelTypeName = "Payment Specification";

    private readonly IEnumerable<int> _pageSizeOptions = BasePagingParams.AllowedSizes;
    private UpsertEditorWrapper<BaseUpsertPaymentSpecReqData> _editorRef = null!;
    private RadzenDataGridWrapper<PaymentSpecResp> _gridWrapperRef = null!;
    private BaseUpsertPaymentSpecReqData? _modelUpsert;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await LoadViewModelFromApi(CurrentPage, CurrentPageSize, string.Empty, string.Empty);
    }

    private void OnAddHandler()
    {
        if (_modelUpsert is not CreatePaymentSpecReq)
        {
            _modelUpsert = new CreatePaymentSpecReq();
        }

        OpenEditorDialog($"Add new {ModelTypeName}");
    }

    private void OnEditHandler(PaymentSpecResp model)
    {
        // If the editing of same item is already in progress, then continue editing it.
        if (_modelUpsert is not UpdatePaymentSpecReq cmd || cmd.Id != model.Id)
        {
            _modelUpsert = new UpdatePaymentSpecReq(model.Id, model.Name, model.CurrencyCode, model.Tags);
        }

        OpenEditorDialog($"Edit {ModelTypeName}: id #{model.Id}");
    }

    protected override ValueTask<bool> Validate() => _editorRef.ValidateEditorAsync();

    protected override void SetServerValidationErrors(IList<Error> errors) =>
        _editorRef.SetServerValidationErrors(errors);

    protected override void CloseEditorDialog()
    {
        base.CloseEditorDialog();
        _editorRef.ResetEditor();
        _modelUpsert = null;
    }

    protected override async ValueTask<bool> HandleUpsertSubmit() => _modelUpsert switch
    {
        UpdatePaymentSpecReq req => await HandleUpdateSubmit(req),
        CreatePaymentSpecReq req => await HandleCreateSubmit(req),
        null => throw new InvalidOperationException("Upsert model is null."),
        _ => throw new InvalidOperationException("Unknown model type."),
    };

    protected override PaymentSpecResp ConvertRequestResponseToVM(PaymentSpecResp dto) => dto;

    protected override PaymentSpecResp ConvertUpdateRequestToVM(IUpdateCommand dto) =>
        (PaymentSpecResp)(UpdatePaymentSpecReq)dto;

    protected override IGetPagingInfoRequest PageCountRequestFactory(int pageSize) =>
        new GetPaymentSpecPagingMetaInfoReq(pageSize);

    private async Task OnDeleteHandler(PaymentSpecResp model)
    {
        var (id, name, _, _) = model;

        if (!await GetUserConfirmation(name, ModelTypeName))
        {
            return;
        }

        SC.IsBusy = true;
        await DeleteModelFromApi(new DeletePaymentSpecReq(id), ModelTypeName);
        SC.IsBusy = false;
    }

    protected override async Task AfterSuccessfulChange(int apiPage) =>
        await _gridWrapperRef.DataGridRef.GoToPage(ToGridPage(apiPage), true);
}
