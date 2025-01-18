using System;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Data;

public class DbInitalizer
{
    public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        await SeedRoles(roleManager);
        await SeedAdmin(userManager, roleManager);
    }

    private static async Task SeedAdmin(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        if (await userManager.FindByEmailAsync("admin@gmail.com") == null)
        {
            var admin = new AppUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, "Admin123-");
            if (!result.Succeeded)
                throw new ValidationException("Cannot create admin: " + result.Errors.FirstOrDefault()?.Description);

            var role = await roleManager.FindByNameAsync(UserRolesEnum.Admin.ToString());
            if (role?.Name == null)
                throw new NotFoundException("Cannot find role: " + UserRolesEnum.Admin);

            result = await userManager.AddToRoleAsync(admin, role.Name);

            if (!result.Succeeded)
                throw new ValidationException("Cannot add user to role: " + result.Errors.FirstOrDefault()?.Description);
        }
    }

    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        if (!roleManager.Roles.Any())
        {
            foreach (var role in Enum.GetNames<UserRolesEnum>())
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
