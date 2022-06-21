using ATABit.Helper.Extensions;
using ATABit.Model.User.Contract;
using ATABit.Shared;
using ATABit.Shared.Dto.Identity;
using Bit.Core.Contracts;
using Bit.Core.Exceptions;
using System.Security.Claims;
using Template.Domain.Shared;

namespace Template.Infrastructure.Identity;

public class UserInfoProvider : IUserInfoProvider
{
    public IUserInformationProvider UserInformationProvider { get; set; } = default!; //Property Injection
    public IRequestInformationProvider RequestInformationProvider { get; set; } = default!; //Property Injection


    public UserDto? User()
    {
        if (!IsAuthenticated())
            return null;

        var firstName = GetClaimFromBitJwtToken(nameof(ATATokenClaims.FirstName));
        var lastName = GetClaimFromBitJwtToken(nameof(ATATokenClaims.LastName));
        var ssoToken = GetClaimFromBitJwtToken(nameof(ATATokenClaims.SSOToken));
        var identityTokenId = GetClaimFromBitJwtToken(nameof(ATATokenClaims.IdentityTokenId));
        var personnelCode = GetClaimFromBitJwtToken(nameof(ATATokenClaims.PersonnelCode));
        var unitName = GetClaimFromBitJwtToken(nameof(ATATokenClaims.UnitName));
        var postTitle = GetClaimFromBitJwtToken(nameof(ATATokenClaims.PostTitle));
        var rahkaranId = GetClaimFromBitJwtToken(nameof(ATATokenClaims.RahkaranId));
        var boxId = GetClaimFromBitJwtToken(nameof(ATATokenClaims.BoxId));

        // ToDo : Add To ATATokenClaims Dto
        var worklocation = GetClaimFromBitJwtToken("WorkLocation");
        var workocationCode = GetClaimFromBitJwtToken("WorkLocationCode");

        return new UserDto
        {
            UserId = UserId(),
            FirstName = firstName,
            LastName = lastName,
            FullName = $"{firstName} {lastName}",
            PersonnelCode = personnelCode!.ToInt(),
            SSOToken = ssoToken,
            RahkaranId = rahkaranId!.ToInt(),
            UnitName = unitName,
            PostTitle = postTitle,
            BoxId = boxId.IsNotNullOrEmpty() ? boxId!.ToInt() : null,
            IdentityTokenId = identityTokenId,
            Roles = Roles(),
            WorkLocation = worklocation,
            WorkLocationCode = workocationCode!.ToInt(),
        };
    }

    public bool IsAuthenticated()
    {
        return UserInformationProvider.IsAuthenticated();
    }

    public int UserId()
    {
        if (!IsAuthenticated())
            throw new UnauthorizedException("این کاربر هنوز لاگین نشده و نمی‌تواند شناسه‌ای داشته باشد");

        var userId = UserInformationProvider.GetCurrentUserId();

        if (string.IsNullOrWhiteSpace(userId))
            throw new UnauthorizedException("این کاربر معتیر نمی‌باشد");

        return userId.ToInt();
    }

    public string IpAddress()
    {
        return RequestInformationProvider.ClientIp;
    }

    public List<string> Roles()
    {
        var stringRoles = GetClaimFromBitJwtToken(ClaimTypes.Role);

        return stringRoles?.Split(",").ToList() ?? new List<string>();
    }

    public List<string> AppClaims()
    {
        return UserInformationProvider.GetBitJwtToken().Claims
            .Where(c => Claims.GetAllAppClaims().ToList().Contains(c.Key))
            .Select(c => c.Value)
            .Distinct()
            .ToList();
    }

    public bool IsAdmin()
    {
        return Roles().Contains(AppConstants.Roles.Administrator);
    }

    private string? GetClaimFromBitJwtToken(string claimName)
    {
        var claimKey = claimName == AppConstants.AuthToken.SSOToken
            ? claimName
            : claimName.ToLowerFirstChar();

        UserInformationProvider.GetBitJwtToken().Claims.TryGetValue(claimKey, out string? claimValue);

        return claimValue;
    }
}