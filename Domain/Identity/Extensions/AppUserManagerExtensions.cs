using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;

namespace domain.Repositories.Extensions;

public static class AppUserManagerExtensions
{
    public static List<AppUser>? FindByClientIdAsync(this UserManager<AppUser> userManager, string clientId)
    {
        return userManager?.Users.Where(user => user.ClientId == clientId).ToList();
    }
}