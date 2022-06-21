using System.Reflection;
using System.Runtime.CompilerServices;
using Bit.Core.Exceptions;

// ReSharper disable InconsistentNaming

namespace Template.Domain.Shared;

public class DefaultRole
{
    public string? Name { get; set; }

    public IEnumerable<string> Claims { get; set; } = Array.Empty<string>();
}

public class DefaultRoles
{
    public static DefaultRole Administrator => new()
    {
        Name = nameof(Administrator),
        Claims = Claims.GetAllAppClaims()
    };
}

public static class Claims //* Do not change Claim values *//
{
    public const string Settings_Permissions_View = nameof(Settings_Permissions_View);

    private static string[]? _claimNames;

    public static string GetClaimDisplayName(string claimType)
    {
        return claimType switch
        {
            Settings_Permissions_View => "مشاهده‌ی کاربران و نقش‌های سامانه",
            _ => throw new DomainLogicException("همه Claimها باید مقدار نمایشی داشته باشند.")
        };
    }

    public static IEnumerable<string> GetAllAppClaims()
    {
        return _claimNames ??= typeof(Claims)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Select(field => (string?)field.GetValue(null))
            .ToArray();
    }

    // Automatic Check for all Claims have DisplayName With Module Initializer
    [ModuleInitializer]
    public static void AutomaticCheckAllClaimsHaveDisplayName()
    {
        var allClaims = GetAllAppClaims();

        foreach (var claim in allClaims)
        {
            GetClaimDisplayName(claim); //Throw error if no display is assigned for the Claim.
        }
    }
}