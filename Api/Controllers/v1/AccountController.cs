using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Casbin.Rbac;
using domain.Dto;
using domain.Identity.Manager;
using domain.Repositories;
using domain.Repositories.Manager;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace khi_robocross_api.Controllers.v1;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private SignInManager<AppUser> _signInManager;
    private IJwtAuthManager _jwtAuthManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager,
        IJwtAuthManager jwtAuthManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _jwtAuthManager = jwtAuthManager;
        _logger = new LoggerFactory().CreateLogger<AccountController>();
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        AppUser appUser = await _userManager.FindByEmailAsync(loginRequest.UserName);
        if (appUser != null)
        {
            await _signInManager.SignOutAsync();
            SignInResult result = await _signInManager.PasswordSignInAsync(appUser, loginRequest.Password, false, false);
            if (result.Succeeded)
            {
                AppRole role = await _roleManager.FindByIdAsync(appUser.Roles.First().ToString());
                
                var claims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, loginRequest.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, appUser.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),

                    new Claim(ClaimTypes.Name,loginRequest.UserName),
                    new Claim(ClaimTypes.NameIdentifier,appUser.Id.ToString()),
                    new Claim(ClaimTypes.Role, role.Name)
                };
                
                var jwtResult = _jwtAuthManager.GenerateTokens(loginRequest.UserName, claims, DateTime.Now);
                
                return Ok(new LoginResult()
                {
                    UserName = loginRequest.UserName,
                    Role = role.Name,
                    AccessToken = jwtResult.AccessToken,
                    RefreshToken = jwtResult.RefreshToken.TokenString
                });
            }
        }

        return Unauthorized("Login Failed: Invalid Email or Password");
    }
    
    [HttpPost("refresh-token")]
    [Authorize]
    public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        try
        {
            var userName = User.Identity?.Name;
            _logger.LogInformation($"User [{userName}] is trying to refresh JWT token.");

            if (string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return Unauthorized();
            }

            var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
            var jwtResult = _jwtAuthManager.Refresh(request.RefreshToken, accessToken, DateTime.Now);
            _logger.LogInformation($"User [{userName}] has refreshed JWT token.");
            return Ok(new LoginResult
            {
                UserName = userName,
                Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            });
        }
        catch (SecurityTokenException e)
        {
            return Unauthorized(e.Message); // return 401 so that the client side can redirect the user to login page
        }
    }

    [HttpPost("impersonation")]
    [Authorize(Roles = "KHI")]
    public async Task<IActionResult> Impersonate([FromBody] ImpersonateRequest request)
    {
        var userName = User.Identity?.Name;
        _logger.LogInformation($"User [{userName}] is trying to impersonate [{request.UserName}].");

        AppUser impersonatedUser = await _userManager.FindByEmailAsync(request.UserName);

        var impersonatedRole = _roleManager.FindByIdAsync(impersonatedUser.Roles.First().ToString()).ToString();
        if (string.IsNullOrWhiteSpace(impersonatedRole))
        {
            _logger.LogInformation(
                $"User [{userName}] failed to impersonate [{request.UserName}] due to the target user not found.");
            return BadRequest($"The target user [{request.UserName}] is not found.");
        }

        if (impersonatedRole == "KHI")
        {
            _logger.LogInformation($"User [{userName}] is not allowed to impersonate another Admin.");
            return BadRequest("This action is not supported.");
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.UserName),
            new Claim(ClaimTypes.Role, impersonatedRole),
            new Claim("OriginalUserName", userName ?? string.Empty)
        };

        var jwtResult = _jwtAuthManager.GenerateTokens(request.UserName, claims, DateTime.Now);
        _logger.LogInformation($"User [{request.UserName}] is impersonating [{request.UserName}] in the system.");
        return Ok(new LoginResult
        {
            UserName = request.UserName,
            Role = impersonatedRole,
            OriginalUserName = userName,
            AccessToken = jwtResult.AccessToken,
            RefreshToken = jwtResult.RefreshToken.TokenString
        });
    }

    [HttpPost("stop-impersonation")]
    [Authorize(Roles = "KHI")]
    public async Task<ActionResult> StopImpersonation()
    {
        var userName = User.Identity?.Name;
        var originalUserName = User.FindFirst("OriginalUserName")?.Value;
        if (string.IsNullOrWhiteSpace(originalUserName))
        {
            return BadRequest("You are not impersonating anyone.");
        }

        _logger.LogInformation($"User [{originalUserName}] is trying to stop impersonate [{userName}].");
        
        AppUser originalUser = await _userManager.FindByEmailAsync(userName);
        var role = _roleManager.FindByIdAsync(originalUser.Roles.First().ToString()).ToString();
 
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, originalUserName),
            new Claim(ClaimTypes.Role, role)
        };

        var jwtResult = _jwtAuthManager.GenerateTokens(originalUserName, claims, DateTime.Now);
        _logger.LogInformation($"User [{originalUserName}] has stopped impersonation.");
        return Ok(new LoginResult
        {
            UserName = originalUserName,
            Role = role,
            OriginalUserName = null,
            AccessToken = jwtResult.AccessToken,
            RefreshToken = jwtResult.RefreshToken.TokenString
        });
    }
    

    [HttpPost("Logout")]
    [Authorize]
    public ActionResult Logout()
    {
        // optionally "revoke" JWT token on the server side --> add the current token to a block-list
        // https://github.com/auth0/node-jsonwebtoken/issues/375
        var userName = User.Identity?.Name;
        _jwtAuthManager.RemoveRefreshTokenByUserName(userName);
        return Ok();
    }
}