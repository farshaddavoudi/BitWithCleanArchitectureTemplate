using Template.Domain.Shared;
using Template.WebUI.Client.Enums;

namespace Template.WebUI.Client.Contracts
{
    public interface INotificationService
    {
        /// <summary>
        /// Show Alert Using AlertifyJS library
        /// </summary>
        /// <param name="notifType">Type of alert including success, error, warning, etc</param>
        /// <param name="message">Message content</param>
        /// <param name="waitSeconds">Time (in seconds) to wait before the message is dismissed, a value of 0 means keep open till clicked.</param>
        /// <param name="position"> Sets a value indicating the position of the notifier instance.</param>
        /// <returns></returns>
        ValueTask AlertAsync(NotificationType notifType, string message, int waitSeconds = AppConstants.AlertDefaultTimeout, AlertifyPosition position = AlertifyPosition.TopCenter);

        /// <summary>
        /// Show Alert Using BlazoredToast library
        /// </summary>
        /// <param name="notifType">Type of alert including success, error, warning, etc</param>
        /// <param name="message">Message content</param>
        /// <param name="waitSeconds"></param>
        /// <param name="showProgressBar"></param>
        /// <returns></returns>
        void Toast(NotificationType notifType, string message, int waitSeconds = 5, bool showProgressBar = true);

        ValueTask AlertGeneralError();
    }
}
