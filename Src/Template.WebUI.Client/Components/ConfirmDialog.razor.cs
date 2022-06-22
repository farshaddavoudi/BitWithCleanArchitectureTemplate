using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using Template.WebUI.Client.Contracts;

namespace Template.WebUI.Client.Components;

public partial class ConfirmDialog
{
    // Props
    private bool _isLoading;
    private bool _isVisible;

    // DIs
    [Inject] public INotificationService NotificationService { get; set; } = default!; //Property Injection
    [Parameter] public string Title { get; set; }
    [Parameter, Required] public string Text { get; set; }
    //[Parameter] public string? SuccessMessage { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; }
    [Parameter] public EventCallback OnConfirm { get; set; }

    [Parameter]
    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            if (value == _isVisible) return;
            _isVisible = value;

            _ = IsVisibleChanged.InvokeAsync(value);
        }
    }

    [Parameter] public EventCallback<bool> IsVisibleChanged { get; set; }

    public async Task OnActionConfirm()
    {
        if (_isLoading) return;

        _isLoading = true;
        try
        {
            await OnConfirm.InvokeAsync();
        }
        finally
        {
            _isLoading = false;
            Hide();
        }

        //ShowMessage(); //This method will call even the Confirm method throws error ;(
    }

    private void Hide()
    {
        IsVisible = false;
    }

    private void ShowMessage()
    {
        //if (string.IsNullOrEmpty(SuccessMessage) is false)
        //{
        //    NotificationService.AlertAsync(NotifType.Success, SuccessMessage);
        //}
    }
}