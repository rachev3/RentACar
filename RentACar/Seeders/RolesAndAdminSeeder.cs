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
            string[] roleNames = { "Admin", "Client" }; 

            IdentityResult roleResult;

            foreach (var roleName in roleNames)
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

            if (admin == null)
            {
                var createPowerUser = await userManager.CreateAsync(adminUser, adminPassword);
                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

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

                if (client == null)
                {
                    var createClient = await userManager.CreateAsync(clientUser, clientPassword);
                    if (createClient.Succeeded)
                    {
                        await userManager.AddToRoleAsync(clientUser, "Client");
                    }
                }
            }

            if (!context.Cars.Any())
            {
                var cars = new[]
                {
                    new Car { LicensePlateNumber = "AB1234CD", Brand = "Toyota", Model = "Corolla", YearOfProduction = new DateOnly(2018, 1, 1), IsRented = false, PricePerDay = 45.50, ImageUrl = "https://media.ed.edmunds-media.com/toyota/corolla/2018/oem/2018_toyota_corolla_sedan_le-eco-wpremium-package_fq_oem_1_1600.jpg" },
                    new Car { LicensePlateNumber = "EF5678GH", Brand = "Honda", Model = "Civic", YearOfProduction = new DateOnly(2020, 1, 1), IsRented = false, PricePerDay = 50.00, ImageUrl = "https://di-honda-enrollment.s3.amazonaws.com/2020-civic-sedan/model-image-2020-civic-sedan-front.png" },
                    new Car { LicensePlateNumber = "IJ9012KL", Brand = "Ford", Model = "Focus", YearOfProduction = new DateOnly(2019, 1, 1), IsRented = false, PricePerDay = 150, ImageUrl = "https://www.autoron.bg/sub/autoobchod.sk/shop/category/ford-focus-2019-up-44808.jpg" },
                    new Car { LicensePlateNumber = "MN3456OP", Brand = "BMW", Model = "3 Series", YearOfProduction = new DateOnly(2021, 1, 1), IsRented = false, PricePerDay = 650, ImageUrl = "https://www.carparisonleasing.co.uk/media/cache/blog_detail_image_1170/cc-uploads/2d6bfe67/BMW%20330e%20profile.jpg" },
                    new Car { LicensePlateNumber = "QR7890ST", Brand = "Audi", Model = "A4", YearOfProduction = new DateOnly(2017, 1, 1), IsRented = false, PricePerDay = 65.00, ImageUrl = "https://hips.hearstapps.com/autoweek/assets/s3fs-public/2017-audi-a4-17.jpg" },
                    new Car { LicensePlateNumber = "UV1234WX", Brand = "Mercedes", Model = "C-Class", YearOfProduction = new DateOnly(2016, 1, 1), IsRented = false, PricePerDay = 75.00, ImageUrl = "https://images.ctfassets.net/c9t6u0qhbv9e/2016MercedesBenzCClassPreviewsummary/91987cf91256143b0aea85c57cb38586/2016_Mercedes-Benz_C-Class_Preview_summaryImage.jpeg" },
                    new Car { LicensePlateNumber = "YZ5678AB", Brand = "Volkswagen", Model = "Golf", YearOfProduction = new DateOnly(2015, 1, 1), IsRented = false, PricePerDay = 130, ImageUrl = "https://www.skyrentacar.eu/wp-content/uploads/CarRentalGallery/1688116825VW-Golf-2012-silver.jpg" },
                    new Car { LicensePlateNumber = "CD9012EF", Brand = "Hyundai", Model = "Elantra", YearOfProduction = new DateOnly(2022, 1, 1), IsRented = false, PricePerDay = 55.00, ImageUrl = "https://www.earnhardthyundai.com/blogs/4378/wp-content/uploads/2021/08/What-Are-the-2022-Hyundai-Elantra-N-Performance-Specs_o.jpg" }
                };

                context.Cars.AddRange(cars);
                await context.SaveChangesAsync();
            }
        }
    }
}
