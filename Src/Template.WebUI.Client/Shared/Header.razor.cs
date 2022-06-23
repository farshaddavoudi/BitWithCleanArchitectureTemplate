using ATA.Broker.SSOSecurity.Contract;
using ATABit.Helper.Extensions;
using ATABit.Shared;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Threading;
using Template.Application.DTOs;
using Template.Domain.Options;
using Template.Domain.Shared;
using Template.WebUI.Client.Extensions;

namespace Template.WebUI.Client.Shared;

public partial class Header
{
    // Props
    public UserDto User { get; set; } = new();
    public string? AvatarPicUrl { get; set; }
    public string? AvatarPicUrlInProfileModal { get; set; }
    public bool IsOpenProfileWindow { get; set; }
    public bool IsOpenNotificationWindow { get; set; }
    public bool IsOpenAppVersioningWindow { get; set; }
    public List<AppVersioningDto> VersionChangeList { get; set; } = new();
    public string AppLatestVersion { get; set; }

    //  Injects
    [Inject] public ILocalStorageService LocalStorageService { get; set; }
    [Inject] public IJSRuntime JsRuntime { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public IATASSOClientService SsoClientService { get; set; }
    [Inject] public ClientAppSettings ClientAppSettings { get; set; }

    // Cascading Parameters
    [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    // Life cycle
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        var isAuthenticated = await AuthenticationStateTask.IsAuthenticated();

        // Only Login page gets into this if block
        if (isAuthenticated is false)
            return;

        var userId = await AuthenticationStateTask.GetUserId();

        var personnelCode = await AuthenticationStateTask.GetPersonnelCode();

        var fullName = await AuthenticationStateTask.GetUserFullName();

        var isMale = await AuthenticationStateTask.GetIsMale();

        User.UserId = userId.ToInt();
        User.PersonnelCode = personnelCode.ToInt();
        AvatarPicUrl = AvatarPicUrlInProfileModal = User.PictureURL;
        User.FullName = fullName;
        User.Gender = isMale ? 1 : 2;

        // Versioning

        //AppLatestVersion = await HttpClient.AppVersioning().GetCurrentVersion(cancellationToken: cancellationToken);

        //var localStorageVersion = await LocalStorageService.GetItemAsStringAsync("AppVersion");

        //if (localStorageVersion is null)
        //{
        //    var userLastVisitedVersionDb = await HttpClient.AppVersioning().GetUserLastVisitedVersion(cancellationToken: cancellationToken);

        //    await ShowVersionChanges(userLastVisitedVersionDb, cancellationToken);

        //    // Set recent version to LocalStorage
        //    await LocalStorageService.SetItemAsync("AppVersion", AppLatestVersion);
        //}

        //else
        //{
        //    if (AppLatestVersion == localStorageVersion)
        //    {
        //        // Do nothing
        //    }
        //    else
        //    {
        //        // Check Db again for confirm
        //        var userLastVisitedVersionDb = await HttpClient.AppVersioning().GetUserLastVisitedVersion(cancellationToken: cancellationToken);

        //        if (userLastVisitedVersionDb != AppLatestVersion)
        //            await ShowVersionChanges(userLastVisitedVersionDb, cancellationToken);
        //        else
        //            await LocalStorageService.SetItemAsStringAsync("AppVersion", AppLatestVersion);
        //    }
        //}
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        //if (firstRender)
        //{
        //    var userId = await AuthenticationStateTask.GetUserId();

        //    User = await HttpClient.User().GetUserById(userId.ToInt());
        //}

        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task ShowVersionChanges(string lastSeenVersion, CancellationToken cancellationToken)
    {
        //if (lastSeenVersion == AppLatestVersion)
        //    return;

        //var allChanges = await HttpClient.AppVersioning().GetVersioningDetails(cancellationToken: cancellationToken);

        //List<AppVersioningDto> changesToShow;

        //if (string.IsNullOrWhiteSpace(lastSeenVersion))
        //{
        //    changesToShow = allChanges
        //        .Where(c => c.Changes.Any(ch => ch.Title.IsNotNullOrEmpty()))
        //        .ToList();
        //}
        //else
        //{
        //    changesToShow = allChanges
        //        .Where(c => c.VersionNo > lastSeenVersion.Replace(".", "").ToInt() && c.Changes.Any(ch => ch.Title.IsNotNullOrEmpty()))
        //        .ToList();
        //}

        //if (changesToShow.Any())
        //{
        //    IsOpenAppVersioningWindow = true;

        //    VersionChangeList = changesToShow;
        //}
    }

    public async Task CloseVersioningWindow()
    {
        //// Set recent version to LocalStorage
        //await LocalStorageService.SetItemAsync("AppVersion", AppLatestVersion);

        //// Update last version seen in Db
        //await HttpClient.AppVersioning().AddOrUpdateUserLastVisitedVersion();

        //IsOpenAppVersioningWindow = false;
    }

    public void OpenProfileWindow()
    {
        if (IsOpenNotificationWindow)
            IsOpenNotificationWindow = false;

        IsOpenProfileWindow = true;
    }

    public void OpenNotificationWindow()
    {
        if (IsOpenProfileWindow)
            IsOpenProfileWindow = false;

        IsOpenNotificationWindow = true;
    }

    private void OnUserImageLoadFailed()
    {
        if (User == null) return;

        AvatarPicUrl = User.IsMale ? "/images/layout/user.png" : "/images/layout/user.png";

        AvatarPicUrlInProfileModal = "/images/layout/user-blue.png";
    }

    private async Task Logout()
    {
        await JsRuntime.DeleteCookieAsync(AppConstants.AuthToken.SSOToken);

        await LocalStorageService.RemoveItemsAsync(new List<string>
        {
            AppConstants.AuthToken.AccessTokenBrowserStorageKey,
            AppConstants.AuthToken.ExpiresAtBrowserStorageKey,
            AppConstants.AuthToken.ExpiresInBrowserStorageKey
        });

        NavigationManager.NavigateTo(SsoClientService.GetSSOLoginPageUrl(ClientAppSettings.URLOptions!.AppURL!));
    }

}