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
            // Get the initial list of cars from the service
                var viewModel = await _carService.GetAll();

            // If no filters are applied, return the full list
            if (filterViewModel == null || filterViewModel.Cars == null)
            {
                return View(viewModel);
            }

            // Start filtering the cars
            var filteredCars = viewModel.Cars.AsQueryable();

            // Apply Brand Filter
            if (!string.IsNullOrWhiteSpace(filterViewModel.BrandFilter))
            {
                filteredCars = filteredCars.Where(c =>
                    c.Brand.Contains(filterViewModel.BrandFilter, StringComparison.OrdinalIgnoreCase));
            }

            // Apply Model Filter
            if (!string.IsNullOrWhiteSpace(filterViewModel.ModelFilter))
            {
                filteredCars = filteredCars.Where(c =>
                    c.Model.Contains(filterViewModel.ModelFilter, StringComparison.OrdinalIgnoreCase));
            }

            // Apply Year of Manufacture Filter
            if (filterViewModel.YearOfManufactureFilter.Year > 1)
            {
                filteredCars = filteredCars.Where(c => c.YearOfManufacture.Year == filterViewModel.YearOfManufactureFilter.Year);
            }

            if (filterViewModel.IsRentedFilter.HasValue)
            {
                if (filterViewModel.IsRentedFilter.Value)
                {
                    filteredCars = filteredCars.Where(c => c.IsRented);
                }
                else
                {
                    filteredCars = filteredCars.Where(c => !c.IsRented);
                }
            }
            // Apply Price Range Filter
            filteredCars = filteredCars.Where(c =>
                c.PricePerDay >= filterViewModel.PriceRangeStart &&
                c.PricePerDay <= filterViewModel.PriceRangeEnd);

            // Update the view model with filtered cars
            viewModel.Cars = filteredCars.ToList();

            // Pass filters back to the view
            viewModel.BrandFilter = filterViewModel.BrandFilter;
            viewModel.ModelFilter = filterViewModel.ModelFilter;
            viewModel.PriceRangeStart = filterViewModel.PriceRangeStart;
            viewModel.PriceRangeEnd = filterViewModel.PriceRangeEnd;
            viewModel.YearOfManufactureFilter = filterViewModel.YearOfManufactureFilter;
            viewModel.IsRentedFilter = filterViewModel.IsRentedFilter;

            return View(viewModel);
        }


    }
}
