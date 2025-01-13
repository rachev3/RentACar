using Microsoft.AspNetCore.Identity;
using RentACar.Models;

namespace RentACar.Seeders
{
    public class RolesAndAdminSeeder
    {
        public static async Task Initialize(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "Client" };   // List of roles

            IdentityResult roleResult;

            // Create roles if they don't exist
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);

                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Admin user
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

            // Check if admin exists; create if not
            if (admin == null)
            {
                var createPowerUser = await userManager.CreateAsync(adminUser, adminPassword);
                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Seed 15 client users
            for (int i = 1; i <= 15; i++)
            {
                var clientUser = new User
                {
                    UserName = $"client{i}",
                    FirstName = $"ClientFirst{i}",
                    MiddleName = $"ClientMiddle{i}",
                    LastName = $"ClientLast{i}",
                    Email = $"client{i}@example.com",
                    PhoneNumber = $"123456789{i:D2}"
                };

                string clientPassword = $"Client@{i:D2}";

                var client = await userManager.FindByEmailAsync(clientUser.Email);

                // Check if the client exists; create if not
                if (client == null)
                {
                    var createClient = await userManager.CreateAsync(clientUser, clientPassword);
                    if (createClient.Succeeded)
                    {
                        await userManager.AddToRoleAsync(clientUser, "Client");
                    }
                }
            }
        }
    }
}
