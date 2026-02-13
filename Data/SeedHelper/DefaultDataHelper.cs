using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;

namespace demoapp.Data.SeedHelper
{
    public static class DefaultDataHelper
    {
        public static async Task SeedRole(IServiceProvider service)
        {
            string strAdminUser = "admin@gmail.com", strPassword ="Pass#123";
            string strClientUser = "client@gmail.com";

            var rolemanager = service.GetRequiredService<RoleManager<IdentityRole>>();
            if (!await rolemanager.RoleExistsAsync("Admin"))
                await rolemanager.CreateAsync(new IdentityRole("Admin"));

            if (!await rolemanager.RoleExistsAsync("Client"))
                await rolemanager.CreateAsync(new IdentityRole("Client"));

            var usermanager = service.GetRequiredService<UserManager<IdentityUser>>();
            if (usermanager != null)
            {
                //Create Admin user
                if (await usermanager.FindByEmailAsync(strAdminUser) == null)
                {
                    await usermanager.CreateAsync(new IdentityUser()
                    {
                        UserName = strAdminUser,
                        Email = strAdminUser,
                        EmailConfirmed = true
                    }, strPassword);
                }

                //Add ROle
                var adminuser = await usermanager.FindByEmailAsync(strAdminUser);
                if (!await usermanager.IsInRoleAsync(adminuser, "Admin"))
                {
                    await usermanager.AddToRoleAsync(adminuser, "Admin");
                }

                //Create ClientUser
                if (await usermanager.FindByEmailAsync(strClientUser) == null)
                {
                    await usermanager.CreateAsync(new IdentityUser()
                    {
                        UserName = strClientUser,
                        Email = strClientUser,
                        EmailConfirmed = true
                    }, strPassword);
                }

                //add role to client
                var newclient = await usermanager.FindByEmailAsync(strClientUser);
                if (newclient != null)
                {
                    if (!await usermanager.IsInRoleAsync(newclient, "Client"))
                    {
                        await usermanager.AddToRoleAsync(newclient, "Client");
                    }
                }
            }
        }

        public static async Task AddClaimToUser(IServiceProvider service)
        {

            var _usermanager = service.GetRequiredService<UserManager<IdentityUser>>();

            //Admin
            var adminuser = await _usermanager.FindByEmailAsync("admin@gmail.com");
            if (adminuser != null)
            {
                await _usermanager.AddClaimAsync(adminuser, new Claim("Permission", "AdminAccess"));
            }
            //Cleint
            var client = await _usermanager.FindByEmailAsync("client@gmail.com");
            if (client != null)
            {
                await _usermanager.AddClaimAsync(client, new Claim("Permission", "ClientAccess"));
            }

            



        }
    }
}
