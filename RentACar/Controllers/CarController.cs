using Microsoft.AspNetCore.Mvc;
using RentACar.Services.Interfaces;
using RentACar.ViewModels.Car;

namespace RentACar.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await _carService.GetAll();
            return View(viewModel);
        }
    }
}
