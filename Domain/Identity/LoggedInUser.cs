using domain.Identity.Manager;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace domain.Identity;

public class LoggedInUser : ILoggedInUser
{
    public string Id { get; }
    public string Username { get; }
    public string Role { get; }
    public string ClientId { get; }
    
    private readonly IJwtAuthManager _jwtAuthManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoggedInUser(IJwtAuthManager jwtAuthManager, IHttpContextAccessor httpContextAccessor)
    {
        _jwtAuthManager = jwtAuthManager;
        _httpContextAccessor = httpContextAccessor;
    }

    private LoggedInUser(string id, string username, string role, string clientId)
    {
        Id = id;
        Username = username;
        Role = role;
        ClientId = clientId;
    }

    public async Task<LoggedInUser> GetLoggedInUser()
    {
        var jwtTokenStr = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
        
        var id = _jwtAuthManager.GetJwtClaims(jwtTokenStr, "jti");
        var username = _jwtAuthManager.GetJwtClaims(jwtTokenStr, "sub");
        var role = _jwtAuthManager.GetJwtClaims(jwtTokenStr, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
        var clientId = _jwtAuthManager.GetJwtClaims(jwtTokenStr, "ClientId");
        return new LoggedInUser(id, username, role, clientId);
    }
}