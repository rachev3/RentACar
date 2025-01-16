using Microsoft.AspNetCore.Identity;
using RentACar.Data;
using RentACar.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Seeders
{
    public class RolesAndAdminSeeder
    {
        public static async Task Initialize(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            string[] roleNames = { "Admin", "Client" }; // List of roles

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

            // Seed 8 cars
            if (!context.Cars.Any())
            {
                var cars = new[]
                {
                    new Car { LicensePlateNumber = "AB1234CD", Brand = "Toyota", Model = "Corolla", YearOfProduction = new DateOnly(2018, 1, 1), IsRented = false, PricePerDay = 45.50 },
                    new Car { LicensePlateNumber = "EF5678GH", Brand = "Honda", Model = "Civic", YearOfProduction = new DateOnly(2020, 1, 1), IsRented = true, PricePerDay = 50.00 },
                    new Car { LicensePlateNumber = "IJ9012KL", Brand = "Ford", Model = "Focus", YearOfProduction = new DateOnly(2019, 1, 1), IsRented = false, PricePerDay = 40.00 },
                    new Car { LicensePlateNumber = "MN3456OP", Brand = "BMW", Model = "3 Series", YearOfProduction = new DateOnly(2021, 1, 1), IsRented = true, PricePerDay = 70.00 },
                    new Car { LicensePlateNumber = "QR7890ST", Brand = "Audi", Model = "A4", YearOfProduction = new DateOnly(2017, 1, 1), IsRented = false, PricePerDay = 65.00 },
                    new Car { LicensePlateNumber = "UV1234WX", Brand = "Mercedes", Model = "C-Class", YearOfProduction = new DateOnly(2016, 1, 1), IsRented = true, PricePerDay = 75.00 },
                    new Car { LicensePlateNumber = "YZ5678AB", Brand = "Volkswagen", Model = "Golf", YearOfProduction = new DateOnly(2015, 1, 1), IsRented = false, PricePerDay = 35.00 },
                    new Car { LicensePlateNumber = "CD9012EF", Brand = "Hyundai", Model = "Elantra", YearOfProduction = new DateOnly(2022, 1, 1), IsRented = false, PricePerDay = 55.00 }
                };

                context.Cars.AddRange(cars);
                await context.SaveChangesAsync();
            }
        }
    }
}
