using ATA.Broker.SSOSecurity.Contract;
using ATABit.Helper.Extensions;
using Bit.Core.Contracts;
using Bit.Core.Exceptions;
using Bit.Core.Models;
using Bit.Data.Contracts;
using Bit.IdentityServer.Implementations;
using Bit.Owin.Implementations;
using IdentityServer3.Core.Models;
using Microsoft.Extensions.Hosting;
using Template.Domain.Entities.Identity;
using Template.Domain.Options;
using Template.Domain.Shared;

namespace Template.Infrastructure.Identity;

public class IdentityUserService : UserService
{
    #region Property Injections

    public IRequestInformationProvider? RequestInformationProvider { get; set; }
    public IDateTimeProvider? DateTimeProvider { get; set; }
    public IRepository<IdentityTokenEntity> Repository { get; set; } = default!;
    public IATASSOClientService SSOClientService { get; set; } = default!;
    //public Service.User.UserService UserService { get; set; } = default!;
    public ServerAppSettings AppSettings { get; set; } = default!;


    #endregion
    public override async Task<BitJwtToken> LocalLogin(LocalAuthenticationContext context, CancellationToken cancellationToken)
    {
        var tokenClaims = new Dictionary<string, string>();

        bool isProduction = AspNetCoreAppEnvironmentsProvider.Current.HostingEnvironment.IsProduction();

        string ssoToken = context.UserName.ToLower();
        var appName = AppMetadata.SSOAppName;
        string clientId = context.SignInMessage.ClientId.ToLower();

        var connectionString = AppSettings.ConnectionStringOptions!.AppDbConnectionString!;

        if (isProduction)
        {
            if (appName != context.Password)
            { // Context.Password here is MasterPassword to direct login
                var masterPassword = context.Password?.Trim();

                if (AppSettings.AuthOptions!.EnableDirectLogin!.Value is false)
                    throw new ForbiddenException("The Direct Login is Disabled in AppSettings");

                if (masterPassword.IsNotNullOrEmpty() && masterPassword != AppSettings.AuthOptions!.MasterPassword!)
                    throw new ForbiddenException("MasterPassword is wrong");
            }
        }

        var userId = await SSOClientService.GetUserIdBySSOTokenAsync(ssoToken, connectionString, cancellationToken);

        if (userId == null)
            throw new DomainLogicException("Login Failed");

        //var user = await UserService.GetUserByIdAsync(userId.Value, cancellationToken);

        var userRoles = await SSOClientService.GetUserRolesAsync(userId.Value, appName, connectionString, cancellationToken);

        // If user do not have any roles, so it mean he/she do not access to the app.
        if (AppSettings.AuthOptions!.IsAppPublic is false && (userRoles is null || userRoles.Count == 0))
            throw new ForbiddenException("شما به این سامانه دسترسی ندارید");

        var identityToken = await Repository.AddAsync(new IdentityTokenEntity
        {
            ClientName = clientId,
            IPAddress = RequestInformationProvider!.ClientIp,
            DeviceName = RequestInformationProvider?.ClientType,
            ExpiresAt = DateTimeProvider!.GetCurrentUtcDateTime() + AppConstants.AuthToken.Lifetime,
            UserId = userId.Value,
            CreatedAt = DateTimeOffset.Now,
            ModifiedAt = DateTimeOffset.Now
        }, cancellationToken);

        //// Standard Claims
        //tokenClaims.Add(ClaimTypes.NameIdentifier, user!.UserId.ToString());
        //tokenClaims.Add(ClaimTypes.Name, $"{user.FirstName} {user.LastName}");

        //if (userRoles?.Count > 0)
        //    tokenClaims.Add(ClaimTypes.Role, string.Join(",", userRoles));

        //var allUserClaims = await UserService.GetUserAllClaims(user.UserId, cancellationToken);
        //foreach (var claim in allUserClaims)
        //    tokenClaims.Add(claim, claim);

        //// ATA Claims
        //tokenClaims.Add(nameof(ATATokenClaims.FirstName).ToLowerFirstChar(), user.FirstName!);
        //tokenClaims.Add(nameof(ATATokenClaims.LastName).ToLowerFirstChar(), user.LastName!);
        //tokenClaims.Add(nameof(ATATokenClaims.SSOToken), ssoToken);
        //tokenClaims.Add(nameof(ATATokenClaims.IdentityTokenId).ToLowerFirstChar(), identityToken.Id.ToString());
        //tokenClaims.Add(nameof(ATATokenClaims.PersonnelCode).ToLowerFirstChar(), user.PersonnelCode.ToString());
        //tokenClaims.Add(nameof(ATATokenClaims.RahkaranId).ToLowerFirstChar(), user.RahkaranId.ToString()!);
        //tokenClaims.Add(nameof(ATATokenClaims.UnitName).ToLowerFirstChar(), user.UnitName ?? string.Empty);
        //tokenClaims.Add(nameof(ATATokenClaims.PostTitle).ToLowerFirstChar(), user.PostTitle ?? string.Empty);
        //tokenClaims.Add(nameof(ATATokenClaims.BoxId).ToLowerFirstChar(), user.BoxId.ToString() ?? string.Empty);
        //tokenClaims.Add(nameof(ATATokenClaims.IsMale).ToLowerFirstChar(), user.Gender == 1 ? "true" : "false");
        //tokenClaims.Add(nameof(ATATokenClaims.WorkLocation).ToLowerFirstChar(), user.WorkLocation ?? string.Empty);
        //tokenClaims.Add(nameof(ATATokenClaims.WorkLocationCode).ToLowerFirstChar(), user.WorkLocationCode.ToString() ?? string.Empty);

        return new BitJwtToken { UserId = userId.Value.ToString(), Claims = tokenClaims };
    }

}