using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ComputerStore.Models
{
    public static class IdentitySeedData
    {
        private const string AdminUserName = "admin";
        private const string AdminPassword = "AdminPassword1234@";

        public static async Task EnsurePopulatedAsync(IApplicationBuilder app)
        {
            var userManager = app.ApplicationServices.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByIdAsync(AdminUserName);
            if (user == null)
            {
                user = new IdentityUser(AdminUserName);
                var result = await userManager.CreateAsync(user, AdminPassword);

                if (!result.Succeeded)
                {
                    throw new IdentitySeedDataException("Cannot create default administrator account", result.Errors);
                }
            }
        }
    }
}
