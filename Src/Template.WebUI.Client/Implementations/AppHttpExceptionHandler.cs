using ATA.Broker.SSOSecurity.Contract;
using ATABit.Helper.Extensions;
using ATABit.Shared;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Template.Domain.Options;
using Template.Domain.Shared;
using Template.WebUI.Client.Contracts;
using Template.WebUI.Client.Enums;

namespace Template.WebUI.Client.Implementations
{
    public class AppHttpExceptionHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly INotificationService _notificationService;
        private readonly NavigationManager _navigationManager;
        private readonly IATASSOClientService _ssoClient;
        private readonly AppSettings _appSettings;

        #region Constructor Injections

        public AppHttpExceptionHandler(ILocalStorageService localStorageService, INotificationService notificationService, NavigationManager navigationManager, IATASSOClientService ssoClient, AppSettings appSettings)
        {
            _localStorageService = localStorageService;
            _notificationService = notificationService;
            _navigationManager = navigationManager;
            _ssoClient = ssoClient;
            _appSettings = appSettings;
        }

        #endregion

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            #region Add Auth header to request

            if (request.Headers.Contains(AppConstants.AuthToken.Scheme) is false)
            {
                var token = await _localStorageService.GetItemAsStringAsync(AppConstants.AuthToken.AccessTokenBrowserStorageKey);

                if (token.IsNotNullOrEmpty())
                    request.Headers.Authorization = new AuthenticationHeaderValue(AppConstants.AuthToken.Scheme, token);
            }

            #endregion

            var response = await base.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode is false)
            {
                var exceptionResultString = await response.Content.ReadAsStringAsync(cancellationToken);

                var exceptionResult = exceptionResultString.DeserializeToModel<ModelErrorWrapper>();

                var message = exceptionResult.Errors.FirstOrDefault()?.Messages.FirstOrDefault();

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        await _notificationService.AlertAsync(NotificationType.Error, string.IsNullOrWhiteSpace(message) ? "Not found" : message);
                        break;

                    case HttpStatusCode.Forbidden:
                        await _notificationService.AlertAsync(NotificationType.Error, string.IsNullOrWhiteSpace(message) ? "You don't have permission" : message);
                        break;

                    case HttpStatusCode.Unauthorized:
                        await _notificationService.AlertAsync(NotificationType.Error, string.IsNullOrWhiteSpace(message) ? "You should login" : message);
                        await Task.Delay(2000, cancellationToken);
                        _navigationManager.NavigateTo(_ssoClient.GetSSOLoginPageUrl(_appSettings.Client.URLOptions!.AppURL!));
                        break;

                    default:
                        await _notificationService.AlertAsync(NotificationType.Error, string.IsNullOrWhiteSpace(message) ? "Some error happened" : message);
                        break;
                }
            }

            return response;
        }
    }
}
