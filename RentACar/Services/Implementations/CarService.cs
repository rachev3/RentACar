using Microsoft.EntityFrameworkCore;
using RentACar.Data;
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
    }
}
