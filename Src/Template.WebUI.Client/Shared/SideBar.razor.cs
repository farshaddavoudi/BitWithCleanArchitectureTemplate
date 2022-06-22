using ATABit.Helper.Extensions;
using Bit.Core.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.ComponentModel.DataAnnotations;
using Template.Domain.Shared;
using Template.WebUI.Client.Extensions;
using Template.WebUI.Client.Pages;
using Template.WebUI.Client.wwwroot;

namespace Template.WebUI.Client.Shared;

public partial class SideBar
{
    // Props
    public List<MenuItem> MenuItems { get; set; } = new()
    {
        // Home
        new()
        {
            Title = "انتخاب غذا",
            IsHome = true,
            URL = PageUrls.SelectFoodPage,
            ActiveIconUrl = IconUrls.SelectFood,
            InactiveIconUrl = IconUrls.SelectFood
        },

        
        // Settings
        new()
        {
            Title = "تنظیمات",
            URL = PageUrls.SettingsRootPath,
            ActiveIconUrl = IconUrls.SettingsInactive,
            InactiveIconUrl = IconUrls.SettingsInactive,
            SubMenuItems = new()
            {
                new("", "مدیریت کاربران", true),
                new(PageUrls.UsersPage, "کاربران دارای نقش"),
                new(PageUrls.RolesPage, "نقش‌های سامانه"),
            },
            Policies = new List<string> { Claims.Settings_Permissions_View }
        }
    };
    public List<SubMenuItem> SubMenuItems { get; set; } = new();
    public bool IsVisibleSubMenuPanel { get; set; }
    public List<string> UserClaims { get; set; } = new();

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    // Life-cycles
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateTask;

        if (authState.User.Identity is null)
            return;

        if (authState.User.Identity.IsAuthenticated)
        {
            UserClaims = await AuthenticationStateTask.GetUserClaims();
        }

        IsVisibleSubMenuPanel = false;

        await base.OnInitializedAsync();
    }

    protected override void OnParametersSet()
    {
        foreach (var menuItem in MenuItems)
        {
            if (menuItem.HasSubMenu)
            {
                if (menuItem.SubMenuItems.Any(subMenuItem => subMenuItem.IsGroupHeader is false && subMenuItem.URL!.Contains(menuItem.URL!) is false))
                    throw new DomainLogicException($"{menuItem.Title} menu has children which their url don't contain the father one!. Please fix it");
            }
        }
    }

    private void OpenSubMenu(MenuItem menu)
    {
        if (menu.HasSubMenu)
        {
            SubMenuItems = menu.SubMenuItems;

            IsVisibleSubMenuPanel = true;
        }
    }

    private void CloseSubMenu()
    {
        IsVisibleSubMenuPanel = false;
    }
}

public class MenuItem
{
    public bool IsHome { get; set; }

    [Required]
    public string? URL { get; set; }
    public string? Href => HasSubMenu ? "javascript:function() { return false; }" : URL;

    [Required]
    public string? Title { get; set; }

    public string? ActiveIconUrl { get; set; }

    public string? InactiveIconUrl { get; set; }

    public bool HasSubMenu => SubMenuItems.Count > 0;

    public List<SubMenuItem> SubMenuItems { get; set; } = new();

    public List<string> Policies { get; set; } = new();

    public bool HasAuthorizeView => Policies.Count > 0;
}

public class SubMenuItem
{
    public SubMenuItem(string? url, string? title, bool isGroupHeader = false, string? policy = null)
    {
        URL = url;
        Title = title;
        IsGroupHeader = isGroupHeader;
        Policy = policy;
    }

    public string? URL { get; set; }

    public string? Title { get; set; }

    public bool IsGroupHeader { get; set; }

    public string? Policy { get; set; }

    public bool HasAuthorizeView => Policy.IsNotNullOrEmpty();
}