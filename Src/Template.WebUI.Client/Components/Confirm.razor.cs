using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace Template.WebUI.Client.Components
{
    public partial class Confirm
    {
        [Parameter]
        public string ConfirmationMessage { get; set; } = "آیا مطمئن هستید؟";

        [CascadingParameter]
        private BlazoredModalInstance ConfirmModal { get; set; }

        private async Task OnConfirm()
        {
            await ConfirmModal.CloseAsync(ModalResult.Ok(true));
        }

        private async Task OnCancel()
        {
            await ConfirmModal.CancelAsync();
        }
    }
}
