using ATABit.Model.User.Contract;
using Bit.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Owin;
using System.Web.Http;
using System.Web.Http.Controllers;
using Template.Domain.Options;
using Template.Domain.Shared;
using AllowAnonymousAttribute = Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute;

namespace Template.WebUI.API.ActionFilters;

public class ATAAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    // Manage roles based on application
    public string[] Claims { get; set; } = Array.Empty<string>();

    public new string[] Roles { get; set; } = Array.Empty<string>();

    // AspNetCore Controllers
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        bool skipAuthorization = context.Filters.OfType<IAllowAnonymousFilter>().Any() ||
                                 context.ActionDescriptor.EndpointMetadata.Any(m => m.GetType().Name == nameof(AllowAnonymousAttribute)) ||
                                 context.ActionDescriptor.EndpointMetadata.Any(m => m.GetType().Name == nameof(AllowAnonymousAttribute));

        if (skipAuthorization) return;

        var userInformationProvider = context.HttpContext.RequestServices.GetRequiredService<IUserInformationProvider>();

        if (!userInformationProvider.IsAuthenticated())
        {
            context.Result = new ForbidResult();
            return;
        }

        var userInfoFromToken = context.HttpContext.RequestServices.GetRequiredService<IUserInfoProvider>();

        var userRoles = userInfoFromToken.Roles();

        var appSettings = context.HttpContext.RequestServices.GetRequiredService<ServerAppSettings>();

        // If user do not have any roles, so it mean he/she do not access to the app.
        if (appSettings.AuthOptions!.IsAppPublic is false && userRoles.Count == 0)
        {
            context.Result = new ForbidResult();
            return;
        }

        if (userRoles.Any(r => r == DefaultRoles.Administrator.Name))
            return;

        var userAppClaims = userInfoFromToken.AppClaims();

        if (!Roles.Any() && !Claims.Any())
            return;

        if (Roles.Any())
        {
            if (!userRoles.Any(ur => Roles.Contains(ur)))
            {
                context.Result = new ForbidResult();
                return;
            }
        }

        if (Claims.Any())
        {
            if (!userAppClaims.Any(c => Claims.Contains(c)))
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }

    // Bit Controllers
    protected override bool IsAuthorized(HttpActionContext actionContext)
    {
        if (actionContext == null)
            throw new ArgumentNullException(nameof(actionContext));

        var resolver = actionContext.Request.GetOwinContext()
            .GetDependencyResolver();

        if (!base.IsAuthorized(actionContext))
            return false;

        var userInfoFromToken = resolver.Resolve<IUserInfoProvider>();

        var userRoles = userInfoFromToken.Roles();

        var userClaims = userInfoFromToken.AppClaims();

        var appSettings = resolver.Resolve<ServerAppSettings>();

        // If user do not have any roles, so it mean he/she do not access to the app.
        if (appSettings.AuthOptions!.IsAppPublic is false && userRoles.Count == 0)
            return false;

        if (userRoles.Any(r => r == DefaultRoles.Administrator.Name))
            return true;

        if (!Roles.Any() && !Claims.Any())
            return true;

        if (Roles.Any())
        {
            if (!userRoles.Any(ur => Roles.Contains(ur)))
                return false;
        }

        if (Claims.Any())
        {
            if (!userClaims.Any(c => Claims.Contains(c)))
                return false;
        }

        return true;
    }
}