using System.Collections.Specialized;
using System.Web;
using Microsoft.AspNetCore.Components;

namespace Template.WebUI.Client.Extensions;

public static class NavigationManagerExtensions
{
    public static NameValueCollection GetQueryStrings(this NavigationManager navigationManager)
    {
        return HttpUtility.ParseQueryString(new Uri(navigationManager.Uri).Query);
    }

    // get single querystring value with specified key
    public static string GetQueryString(this NavigationManager navigationManager, string key)
    {
        return navigationManager.GetQueryStrings()[key];
    }
}