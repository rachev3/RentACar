using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentACar.Models;
using RentACar.ViewModels.User;

namespace RentACar.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;

        public AdminController(UserManager<User> userManager)
        {
            _userManager = userManager;
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
    }
}

