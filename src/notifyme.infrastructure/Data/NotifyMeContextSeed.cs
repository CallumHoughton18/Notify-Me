using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using notifyme.infrastructure.Identity;

namespace notifyme.infrastructure.Data
{
    public class NotifyMeContextSeed
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(shared.Authorization.Constants.Roles.ADMINISTRATORS));

            string adminUserName = "admin@test.com";
            var adminUser = new AppUser() { UserName = adminUserName, Email = adminUserName };
            await userManager.CreateAsync(adminUser, "Ch@ngeMe1!");
            adminUser = await userManager.FindByNameAsync(adminUserName);
            await userManager.AddToRoleAsync(adminUser, shared.Authorization.Constants.Roles.ADMINISTRATORS);
        }
    }
}