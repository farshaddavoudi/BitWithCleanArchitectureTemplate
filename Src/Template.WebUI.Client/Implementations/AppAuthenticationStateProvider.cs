using System.Security.Claims;
using System.Threading;
using ATA.Broker.SSOSecurity.Contract;
using ATA.Broker.SSOSecurity.Dtos;
using ATABit.Shared.Extensions;
using Bit.Core.Exceptions;
using Bit.Http.Contracts;
using Bit.Utils.Extensions;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using Template.Domain.Options;
using Template.Domain.Shared;
using Template.WebUI.Client.Contracts;
using Template.WebUI.Client.Enums;
using Template.WebUI.Client.Extensions;

namespace Template.WebUI.Client.Implementations;

public class AppAuthenticationStateProvider : AuthenticationStateProvider
{
    private const string DevDefaultUserPersonnelCode = "991126";

    private readonly ITokenProvider _tokenProvider;
    private readonly ILocalStorageService _localStorageService;
    private readonly ISecurityService _securityService;
    private readonly IWebAssemblyHostEnvironment _hostEnvironment;
    private readonly INotificationService _notificationService;
    private readonly IATASSOClientService _ssoClientService;
    private readonly IJSRuntime _jsRuntime;
    private readonly NavigationManager _navigationManager;
    private readonly AppSettings _appSettings;

    #region Constructor Injections

    public AppAuthenticationStateProvider(ITokenProvider tokenProvider,
        ILocalStorageService localStorageService,
        ISecurityService securityService,
        IWebAssemblyHostEnvironment hostEnvironment,
        INotificationService notificationService,
        IJSRuntime jsRuntime,
        IATASSOClientService ssoClientService,
        NavigationManager navigationManager,
        AppSettings appSettings)
    {
        _tokenProvider = tokenProvider;
        _localStorageService = localStorageService;
        _securityService = securityService;
        _hostEnvironment = hostEnvironment;
        _notificationService = notificationService;
        _jsRuntime = jsRuntime;
        _ssoClientService = ssoClientService;
        _navigationManager = navigationManager;
        _appSettings = appSettings;
    }

    #endregion

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        #region Manage Login with different user using UserId and MasterPassword

        string? localStoragePersonnelCode = "";
        string? localStorageMasterPassword = "";

        try
        {
            localStoragePersonnelCode = await _localStorageService.GetItemAsStringAsync(AppConstants.AuthToken.PersonnelCode);
            localStorageMasterPassword = await _localStorageService.GetItemAsStringAsync(AppConstants.AuthToken.MasterPassword);
        }
        catch (Exception)
        {
            // Ignored
        }

        if (_hostEnvironment.IsDevelopment())
        {
            if (string.IsNullOrWhiteSpace(localStoragePersonnelCode))
                localStoragePersonnelCode = DevDefaultUserPersonnelCode;

            var ssoToken = await _ssoClientService.GetUserTokenByPersonnelCodeAsync($"https://security.app.ataair.ir/api/Authentication/LoginByOtherPersonUsingPersonCode?perscode={localStoragePersonnelCode}&version=1&remarks=ata@t@", CancellationToken.None);

            if (string.IsNullOrWhiteSpace(ssoToken))
            {
                _notificationService.Toast(NotificationType.Error, "Something wrong. No user found with provided PersonnelCode in LocalStorage");
                throw new BadRequestException("Something wrong. No user found with provided PersonnelCode in LocalStorage");
            }

            await _securityService.LoginWithCredentials(ssoToken, AppMetadata.SSOAppName, AppConstants.WebApp.ClientId, AppConstants.WebApp.Secret);
        }
        else //IsProduction()
        {
            if (localStoragePersonnelCode.IsNotNullOrEmpty() && localStorageMasterPassword.IsNotNullOrEmpty())
            {
                try
                {
                    var ssoToken = await _ssoClientService.GetUserTokenByPersonnelCodeAsync($"https://security.app.ataair.ir/api/Authentication/LoginByOtherPersonUsingPersonCode?perscode={localStoragePersonnelCode}&version=1&remarks=ata@t@", CancellationToken.None);

                    if (string.IsNullOrWhiteSpace(ssoToken))
                    {
                        _notificationService.Toast(NotificationType.Error, "Something wrong. No user found with provided PersonnelCode in LocalStorage. For SSO, remove these local storage");
                        throw new BadRequestException("Something wrong. No user found with provided PersonnelCode in LocalStorage. For SSO, remove these local storage");
                    }

                    await _securityService.LoginWithCredentials(ssoToken, localStorageMasterPassword, AppConstants.WebApp.ClientId, AppConstants.WebApp.Secret);
                }
                catch (Exception)
                {
                    await _notificationService.AlertAsync(NotificationType.Error, "PersonnelCode or MasterPassword is wrong in LocalStorage. For SSO, remove these local storage");

                    throw;
                }
            }
        }

        #endregion

        // 1. Get JwtToken by calling _tokenProvider.GetTokenAsync()
        var token = await _tokenProvider.GetTokenAsync();

        var isTokenExpired = false;

        if (token is not null)
        {
            var expiresAtUtc = new DateTimeOffset(new DateTime(token.expires_at), TimeSpan.Zero);

            isTokenExpired = expiresAtUtc < DateTimeOffset.UtcNow;
        }

        if (isTokenExpired || string.IsNullOrWhiteSpace(token?.access_token))
        {
            // Get SSOToken from Cookie
            var ssoToken = await _jsRuntime.GetCookieAsync(AppConstants.AuthToken.SSOToken);

            if (string.IsNullOrWhiteSpace(ssoToken))
                await NavigateToSSOLoginPage();

            // Call SSO client to confirm token
            UserByTokenData? userBySSOToken = null;
            try
            {
                userBySSOToken = await _ssoClientService.GetUserByTokenAsync(ssoToken!, default);
            }
            catch
            {
                await NavigateToSSOLoginPage();
            }

            // No valid response or token has been expired
            if (userBySSOToken is null)
            {
                await NavigateToSSOLoginPage();
            }

            // SSOToken is valid at this point
            try
            {
                token = await _securityService.LoginWithCredentials(ssoToken!, AppMetadata.SSOAppName, AppConstants.WebApp.ClientId, AppConstants.WebApp.Secret);
            }
            catch
            {
                await NavigateToSSOLoginPage();
            }
        }

        // Decode the JwtToken and export Claims / Roles from it 
        var claims = token.access_token.ParseTokenClaims();

        Console.WriteLine($"MyInfo={claims.SerializeToJson()}");

        var roles = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (roles.IsNotNullOrEmpty())
        {
            foreach (var role in roles!.Split(","))
            {
                if (!claims.Any(c => c.Type == ClaimTypes.Role && c.Value == role))
                    claims.Add(new Claim(ClaimTypes.Role, role));
            }
        }

        // Creates ClaimsIdentity
        var claimsIdentity = new ClaimsIdentity(claims, "Bearer");

        // Creates ClaimsPrinciple
        var claimsPrinciple = new ClaimsPrincipal(claimsIdentity);

        return new AuthenticationState(claimsPrinciple);
    }

    private async Task NavigateToSSOLoginPage()
    {
        if (_hostEnvironment.IsDevelopment())
        {
            var securitySSOToken =
                await _ssoClientService.GetUserTokenByPersonnelCodeAsync($"https://security.app.ataair.ir/api/Authentication/LoginByOtherPersonUsingPersonCode?perscode={DevDefaultUserPersonnelCode}&version=1&remarks=ata@t@", CancellationToken.None);

            // Set auth cookie
            await _jsRuntime.SetCookieAsync(AppConstants.AuthToken.SSOToken, securitySSOToken, AppConstants.AuthToken.Lifetime.TotalMinutes);
        }
        else
        {
            // Remove SSOToken
            await _jsRuntime.DeleteCookieAsync(AppConstants.AuthToken.SSOToken);

            // Navigate to SSOLogin
            _navigationManager.NavigateTo(_ssoClientService.GetSSOLoginPageUrl(_appSettings.Client!.URLOptions!.AppURL!));
        }
    }
}

