using Bit.Utils.Extensions;
using Blazored.Toast;
using Blazored.Toast.Services;
using Microsoft.JSInterop;
using Template.Domain.Shared;
using Template.WebUI.Client.Components;
using Template.WebUI.Client.Contracts;
using Template.WebUI.Client.Enums;

namespace Template.WebUI.Client.Implementations;

public class NotificationService : INotificationService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly IToastService _toastService;

    public NotificationService(IJSRuntime jsRuntime, IToastService toastService)
    {
        _jsRuntime = jsRuntime;
        _toastService = toastService;
    }

    public async ValueTask AlertAsync(NotificationType notifType, string message, int waitSeconds = AppConstants.AlertDefaultTimeout, AlertifyPosition position = AlertifyPosition.TopCenter)
    {
        // Set position
        await _jsRuntime.InvokeVoidAsync("alertify.set", "notifier", "position", position.ToDisplayName()!);

        // Show alert
        string jsIdentifier = notifType switch
        {
            NotificationType.Success => "alertify.success",
            NotificationType.Error => "alertify.error",
            NotificationType.Message => "alertify.message",
            NotificationType.Notify => "alertify.notify",
            NotificationType.Warning => "alertify.warning",
            _ => throw new ArgumentOutOfRangeException(nameof(notifType), notifType, null)
        };

        await _jsRuntime.InvokeVoidAsync(jsIdentifier, message, waitSeconds);
    }

    public void Toast(NotificationType notifType, string message, int waitSeconds = 5, bool showProgressBar = true)
    {
        // Toast Params
        var toastParameters = new ToastParameters();
        // ReSharper disable once EntityNameCapturedOnly.Local
        Toast toastComponent;
        toastParameters.Add(nameof(toastComponent.Message), message);
        toastParameters.Add(nameof(toastComponent.NotifType), notifType);

        // Toast Settings
        var toastSettings = new ToastInstanceSettings(waitSeconds, showProgressBar);

        _toastService.ShowToast<Toast>(toastParameters, toastSettings);
    }

    public ValueTask AlertGeneralError()
    {
        return AlertAsync(NotificationType.Error, "Some error happened");
    }
}