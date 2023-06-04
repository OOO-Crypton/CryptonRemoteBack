using System.Security.Claims;

namespace CryptonRemoteBack.Extensions
{
    public static class UserExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user
                .FindFirst(x => x.Type == "UserId")?.Value
                ?? throw new InvalidOperationException($"Claim `UserId` not found");
        }

        public static string? GetUserRole(this ClaimsPrincipal user)
        {
            return user.FindFirst("Role")?.Value;
        }
    }
}
