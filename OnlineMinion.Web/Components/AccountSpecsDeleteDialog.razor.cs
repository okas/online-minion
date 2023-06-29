using Microsoft.AspNetCore.Components;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Web.Shared;

namespace OnlineMinion.Web.Components;

public partial class AccountSpecsDeleteDialog : ComponentBase
{
    private ModalDialog _modalRef = null!;
    private string? _modalTitle;

    [Parameter]
    [EditorRequired]
    public AccountSpecResp Model { get; set; }

    [Parameter]
    [EditorRequired]
    public EventCallback OnConfirm { get; set; }

    public void OpenForDelete(int id)
    {
        _modalTitle = $"DeleteBtn Account Specification: id#{id}";
        _modalRef.Open();
    }

    public void Reset()
    {
        _modalRef.Close();
        _modalTitle = null;
    }
}
