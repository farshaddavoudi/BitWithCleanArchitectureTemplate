using ATABit.Shared.Dto.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Template.WebUI.Client.Extensions
{
    public static class AuthStateTaskExtensions
    {
        public static async Task<bool> IsAuthenticated(this Task<AuthenticationState> authStateTask)
        {
            var authState = await authStateTask;

            return authState.User.Identity is not null;
        }

        public static async Task<string> GetUserId(this Task<AuthenticationState> authStateTask)
        {
            var authState = await authStateTask;

            return authState.User.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value)
                .FirstOrDefault();
        }

        public static async Task<string> GetUserFullName(this Task<AuthenticationState> authStateTask)
        {
            var authState = await authStateTask;

            return authState.User.Claims
                .Where(c => c.Type == ClaimTypes.Name)
                .Select(c => c.Value)
                .FirstOrDefault();
        }

        public static async Task<string> GetPersonnelCode(this Task<AuthenticationState> authStateTask)
        {
            var authState = await authStateTask;

            return authState.User.Claims
                .Where(c => c.Type.Equals(nameof(ATATokenClaims.PersonnelCode), StringComparison.OrdinalIgnoreCase))
                .Select(c => c.Value)
                .FirstOrDefault();
        }

        public static async Task<bool> GetIsMale(this Task<AuthenticationState> authStateTask)
        {
            var authState = await authStateTask;

            var isMaleClaim = authState.User.Claims.Single(c => c.Type.Equals(nameof(ATATokenClaims.IsMale), StringComparison.OrdinalIgnoreCase));

            return isMaleClaim.Value.Equals("true", StringComparison.OrdinalIgnoreCase);
        }

        public static async Task<List<string>> GetUserRoles(this Task<AuthenticationState> authStateTask)
        {
            var authState = await authStateTask;

            return authState.User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();
        }

        public static async Task<List<string>> GetUserClaims(this Task<AuthenticationState> authStateTask)
        {
            var authState = await authStateTask;

            return authState.User.Claims
                .Select(c => c.Value)
                .ToList();
        }
    }
}