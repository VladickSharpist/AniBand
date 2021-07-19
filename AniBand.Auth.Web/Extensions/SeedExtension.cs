using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using AniBand.Domain;
using AniBand.Domain.Models;

namespace AniBand.Auth.Web.Extensions
{
    public static class SeedExtension
    {
        public static async Task SeedAllAsync(IServiceProvider services)
        {
            var userManager = services.GetService<UserManager<User>>();
            var roleManager = services.GetService<RoleManager<IdentityRole>>();
            await SeedRolesAsync(roleManager);
            await SetAdminAccountAsync(userManager);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
        }

        private static async Task SetAdminAccountAsync(UserManager<User> userManager)
        {
            var admin = new User()
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                RegistrationDate = DateTime.Now
            };
            var user = await userManager.FindByEmailAsync(admin.Email.ToUpper());
            if (user == null)
            {
                await userManager.CreateAsync(admin, "adminadmin");
                await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
            }
            user = await userManager.FindByEmailAsync(admin.Email.ToUpper());
        }
    }
}