using Microsoft.JSInterop;

namespace Template.WebUI.Client.Extensions;

public static class JsRuntimeExtensions
{
    public static ValueTask AlertAsync(this IJSRuntime jsRuntime, string alertMessage)
    {
        return jsRuntime.InvokeVoidAsync("alert", alertMessage);
    }

    public static ValueTask SetCookieAsync(this IJSRuntime jsRuntime, string cookieName, string cookieValue, double expireMinutes = 525600)
    {
        return jsRuntime.InvokeVoidAsync("setCookie", cookieName, cookieValue, expireMinutes);
    }

    public static ValueTask<string> GetCookieAsync(this IJSRuntime jsRuntime, string cookieName)
    {
        return jsRuntime.InvokeAsync<string>("getCookie", cookieName);
    }

    public static ValueTask<string> DeleteCookieAsync(this IJSRuntime jsRuntime, string cookieName)
    {
        return jsRuntime.InvokeAsync<string>("deleteCookie", cookieName);
    }

    public static ValueTask OpenInNewTabAsync(this IJSRuntime jsRuntime, string url)
    {
        return jsRuntime.InvokeVoidAsync("open", url, "_blank");
    }

    public static ValueTask ChangeAddressBarUrlAsync(this IJSRuntime jsRuntime, string newUrl)
    {
        return jsRuntime.InvokeVoidAsync("changeAddressBarUrl", newUrl);
    }

    public static ValueTask AddCSSClassByElementIdAsync(this IJSRuntime jsRuntime, string elementId, string className)
    {
        return jsRuntime.InvokeVoidAsync("addClassToElementById", elementId, className);
    }

    public static ValueTask SetFocusAsync(this IJSRuntime jsRuntime, string elementId)
    {
        return jsRuntime.InvokeVoidAsync("setFocus", elementId);
    }

}