using Core.Entities.Identity;
using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string RetrieveEmailFromPrincipal(this ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);

            return email;
        }
    }
}
