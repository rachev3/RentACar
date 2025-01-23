using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentACar.Data;
using RentACar.Models;
using RentACar.Seeders;
using RentACar.Services.Implementations;
using RentACar.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IRentService, RentService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Ensure the database is created
    dbContext.Database.EnsureCreated();

    await RolesAndAdminSeeder.Initialize(userManager, roleManager, dbContext);
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();



