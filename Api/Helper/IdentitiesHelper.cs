using System.Security.Claims;

namespace khi_robocross_api.Helper;

public static class IdentitiesHelper
{
    public static string GetUserIdFromClaimPrincipal(ClaimsPrincipal user)
    {
        IEnumerable<ClaimsIdentity> claimsIdentities = user.Identities;
        string userId = "";

        foreach (var claimsIdentity in claimsIdentities.ToList())
        { 
            userId = claimsIdentity.FindFirst("jti")?.Value;
        }
        
        return userId;
    }
}