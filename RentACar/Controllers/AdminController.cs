using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentACar.Models;
using RentACar.Services.Interfaces;
using RentACar.ViewModels.Car;
using RentACar.ViewModels.User;

namespace RentACar.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ICarService _carService;

        public AdminController(UserManager<User> userManager, ICarService carService)
        {
            _userManager = userManager;
            _carService = carService;
        }
        public async Task<IActionResult> Index(string searchQuery = "", int usersPerPage = 3, int page = 1)
        {
            var users = _userManager.Users.ToList();

            var clients = new List<ClientAdminPanelViewModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains("Admin"))
                {
                    clients.Add(new ClientAdminPanelViewModel
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        MiddleName = user.MiddleName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        UserName = user.UserName
                    });
                }
            }

            var filteredClients = string.IsNullOrWhiteSpace(searchQuery)
                ? clients
                : clients.Where(c =>
                    (c.FirstName?.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (c.MiddleName?.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (c.LastName?.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ?? false))
                    .ToList();

            var paginatedClients = filteredClients
                .Skip((page - 1) * usersPerPage)
                .Take(usersPerPage)
                .ToList();

            var totalPages = (int)Math.Ceiling((double)filteredClients.Count / usersPerPage);

            var model = new ClientsListAdminPanelViewModel
            {
                Clients = paginatedClients,
                SearchQuery = searchQuery,
                UsersPerPage = usersPerPage,
                CurrentPage = page,
                TotalPages = totalPages
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ClientsTable", model);
            }

            return View(model);
        }




        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            // Find the user by ID
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Delete the user
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index"); // Redirect to a user list page
            }

            // Handle errors
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View("Error");
        }

        public async Task<IActionResult> Cars()
        {
            var viewModel = await _carService.GetAll();
            return View("CarsManagementPage", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCar(int carId)
        {
            // Retrieve the car from the service
            var car = await _carService.GetById(carId);

            // Validate if the car is rented
            if (car == null)
            {
                TempData["Error"] = "Car not found.";
                return RedirectToAction("Cars");
            }

            if (car.IsRented)
            {
                TempData["Error"] = "Cannot delete a car that is currently rented.";
                return RedirectToAction("Cars");
            }

            // Perform deletion
            await _carService.Delete(carId);
    
            return RedirectToAction("Cars");
        }

        [HttpGet]
        public async Task<IActionResult> EditCar(int carId)
        {
            var viewModel = await _carService.GetById(carId);

            // Validate if the car is rented
            if (viewModel == null)
            {
                TempData["Error"] = "Car not found.";
                return RedirectToAction("Cars");
            }

            if (viewModel.IsRented)
            {
                TempData["Error"] = "Cannot edit a car that is currently rented.";
                return RedirectToAction("Cars");
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditCarSave(CarViewModel viewModel)
        {
            // Retrieve the car from the service
            var existingCar = await _carService.GetById(viewModel.CarId);

            // Validate if the car is rented
            if (existingCar == null)
            {
                TempData["Error"] = "Car not found.";
                return RedirectToAction("Cars");
            }

            if (existingCar.IsRented)
            {
                TempData["Error"] = "Cannot save changes to a car that is currently rented.";
                return RedirectToAction("Cars");
            }

            // Perform update
            await _carService.Update(viewModel);
            return RedirectToAction("Cars");
        }

        [HttpGet]
        public async Task<IActionResult> CreateCar()
        {
            CarViewModel viewModel = new();    
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCarSave(CarViewModel viewModel)
        {
            await _carService.Create(viewModel);
            return RedirectToAction("Cars");
        }

    }
}

