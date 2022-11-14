using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindByUserByClaimsPrincipalWithAddressAsync(
            this UserManager<AppUser> input, ClaimsPrincipal user)
        {
            // mejor q el de abajo
            //var email = User.FindFirstValue(ClaimTypes.Email);
            var email = user?.Claims?.FirstOrDefault( x => x.Type == ClaimTypes.Email )?.Value;

            return await input.Users.Include(u => u.Address).SingleOrDefaultAsync(u => u.Email == email);
        }

        public static async Task<AppUser> FindByEmailFromClaimsPrincipal(this UserManager<AppUser> input,
                ClaimsPrincipal user)
        {
            // mejor q el de abajo
            //var email = User.FindFirstValue(ClaimTypes.Email);
            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            return await input.Users.SingleOrDefaultAsync(u => u.Email == email);
        }
    }
}
