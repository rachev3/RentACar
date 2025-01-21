using Microsoft.EntityFrameworkCore;
using RentACar.Data;
using RentACar.Models;
using RentACar.Services.Interfaces;
using RentACar.ViewModels.Car;

namespace RentACar.Services.Implementations
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext _context;
        public CarService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<CarListViewModel> GetAll()
        {
            var result = await _context.Cars.ToListAsync();
            CarListViewModel viewModel = new CarListViewModel();
            viewModel.Cars = result.Select(car => new CarViewModel
            {
                CarId = car.CarId,
                Brand = car.Brand,
                Model = car.Model,
                LicensePlateNumber = car.LicensePlateNumber,
                YearOfManufacture = car.YearOfProduction,
                IsRented = car.IsRented,
                PricePerDay = car.PricePerDay
            }).ToList();
            return viewModel;
        }
        public async Task Delete(int carId)
        {
           var result = await _context.Cars.FirstOrDefaultAsync(car => car.CarId == carId);
            _context.Cars.Remove(result);
            await _context.SaveChangesAsync();
        }

        public async Task<CarViewModel> GetById(int carId)
        {
            var result =  await _context.Cars.FirstOrDefaultAsync(car => car.CarId == carId);
            CarViewModel viewModel = new CarViewModel
            {
                CarId = result.CarId,
                Brand = result.Brand,
                Model = result.Model,
                LicensePlateNumber = result.LicensePlateNumber,
                YearOfManufacture = result.YearOfProduction,
                IsRented = result.IsRented,
                PricePerDay = result.PricePerDay
            };
            return viewModel;
        }

        public async Task Update(CarViewModel viewModel)
        {
            var car = await _context.Cars.FirstOrDefaultAsync(car => car.CarId == viewModel.CarId);
            car.Brand = viewModel.Brand;
            car.Model = viewModel.Model;
            car.LicensePlateNumber = viewModel.LicensePlateNumber;
            car.YearOfProduction = viewModel.YearOfManufacture;
            car.IsRented = viewModel.IsRented;
            car.PricePerDay = viewModel.PricePerDay;

            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
        }

        public async Task Create(CarViewModel viewModel)
        {
            Car car = new Car{

                Brand = viewModel.Brand,
                Model = viewModel.Model,
                LicensePlateNumber = viewModel.LicensePlateNumber,
                YearOfProduction = viewModel.YearOfManufacture,
                IsRented = viewModel.IsRented,
                PricePerDay = viewModel.PricePerDay
            };
            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
            
        }
    }
}
