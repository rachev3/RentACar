using Microsoft.AspNetCore.Mvc;
using RentACar.ViewModels.Car;

namespace RentACar.Controllers
{
    public class Car : Controller
    {
        public IActionResult Index()
        {
            CarListViewModel model = new CarListViewModel();
            return View(model);
        }
    }
}
