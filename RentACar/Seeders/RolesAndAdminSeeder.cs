using Microsoft.AspNetCore.Identity;
using RentACar.Models;

namespace RentACar.Seeders
{
    public class RolesAndAdminSeeder
    {
        public static async Task Initialize(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "Client" };   // списък с роли

            IdentityResult roleResult;

            foreach (var roleName in roleNames)      // добавя ролите ако не съществуват
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);

                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var adminUser = new User
            {
                UserName = "Administrator",
                FirstName = "Admin",
                MiddleName = "Adminov",
                LastName = "Adminov",
                Email = "admin@example.com",
            };
         
            string adminPassword = "Admin@123";
           
            var admin = await userManager.FindByEmailAsync("admin@example.com");

            //проверява дали съшествуват такъв потребител и го създава ако не съществува

            if (admin == null)
            {
                var createPowerUser = await userManager.CreateAsync(adminUser, adminPassword);
                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
          
        }
    }
}
