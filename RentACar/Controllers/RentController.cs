using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentACar.Models;
using RentACar.Services.Interfaces;
using RentACar.ViewModels.Rent;

namespace RentACar.Controllers
{
    public class RentController : Controller
    {
        private readonly IRentService _rentService;
        private readonly UserManager<User> _userManager;

        public RentController(IRentService rentService, UserManager<User> userManager)
        {
            _rentService = rentService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Create(int carId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(); // Handle the case where the user is not authenticated
            }

            var viewModel = await _rentService.CreatingRentAction(carId, user.Id);
            if (viewModel == null)
            {
                return NotFound(); // Handle the case where the view model could not be created
            }

            return View("CreateRent", viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmRent(CreateRentViewModel viewModel)
        {   
             await _rentService.ConfirmRent(viewModel);
             return RedirectToAction("Index","Car");
        }
    }
}
