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

        public async Task<IActionResult> Index(CarListViewModel filterViewModel)
        {
            var viewModel = await _carService.GetAll();

            if (filterViewModel == null || filterViewModel.Cars == null)
            {
                return View(viewModel);
            }

            var filteredCars = viewModel.Cars.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterViewModel.BrandFilter))
            {
                filteredCars = filteredCars.Where(c =>
                    c.Brand.Contains(filterViewModel.BrandFilter, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(filterViewModel.ModelFilter))
            {
                filteredCars = filteredCars.Where(c =>
                    c.Model.Contains(filterViewModel.ModelFilter, StringComparison.OrdinalIgnoreCase));
            }

            if (filterViewModel.StartYearFilter.HasValue)
            {
                filteredCars = filteredCars.Where(c => c.YearOfManufacture.Year >= filterViewModel.StartYearFilter.Value);
            }

            if (filterViewModel.EndYearFilter.HasValue)
            {
                filteredCars = filteredCars.Where(c => c.YearOfManufacture.Year <= filterViewModel.EndYearFilter.Value);
            }

            if (filterViewModel.IsRentedFilter.HasValue)
            {
                filteredCars = filteredCars.Where(c => c.IsRented == filterViewModel.IsRentedFilter.Value);
            }

            filteredCars = filteredCars.Where(c =>
                c.PricePerDay >= filterViewModel.PriceRangeStart &&
                c.PricePerDay <= filterViewModel.PriceRangeEnd);

            viewModel.Cars = filteredCars.ToList();

            viewModel.BrandFilter = filterViewModel.BrandFilter;
            viewModel.ModelFilter = filterViewModel.ModelFilter;
            viewModel.PriceRangeStart = filterViewModel.PriceRangeStart;
            viewModel.PriceRangeEnd = filterViewModel.PriceRangeEnd;
            viewModel.StartYearFilter = filterViewModel.StartYearFilter;
            viewModel.EndYearFilter = filterViewModel.EndYearFilter;
            viewModel.IsRentedFilter = filterViewModel.IsRentedFilter;

            return View(viewModel);
        }


    }
}
