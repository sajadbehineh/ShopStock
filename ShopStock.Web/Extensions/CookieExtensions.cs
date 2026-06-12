using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

namespace ShopStock.Web.Extensions
{
    public static class CookieExtensions
    {
        public static async Task RefreshUserClaimsAsync(this Controller controller,
            string fullName,
            string mobile,
            string profilePicture)
        {
            var httpContext = controller.HttpContext;

            // Step 1: Authenticate the current user to get existing claims
            var authenticateResult = await httpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //var identity = authenticateResult?.Principal?.Identity as ClaimsIdentity
            //               ?? httpContext.User.Identity as ClaimsIdentity;
            if ((authenticateResult?.Principal?.Identity ?? httpContext.User.Identity) is not ClaimsIdentity identity)
                return;

            // Step 2: Keep existing claims except the ones we want to update
            var properties = authenticateResult.Properties ?? new AuthenticationProperties();

            // Step 3: Replace or add the specific claims with new values
            void ReplaceClaim(string claimType, string newValue)
            {
                var existingClaim = identity.FindFirst(claimType);
                if (existingClaim != null)
                {
                    identity.RemoveClaim(existingClaim);
                }
                identity.AddClaim(new Claim(claimType, newValue ?? string.Empty));
            }

            // Step 4: Update the claims with new values

            //ReplaceClaim(ClaimTypes.Name, userName);
            ReplaceClaim("FullName", fullName);
            ReplaceClaim("Mobile", mobile);
            ReplaceClaim("ProfilePicture", profilePicture);

            // Step 5: Silent Sign in the user again with the updated claims
            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                properties);
        }
    }
}
