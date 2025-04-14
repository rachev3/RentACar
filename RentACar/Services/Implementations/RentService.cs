using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentACar.Data;
using RentACar.Models;
using RentACar.Services.Interfaces;
using RentACar.ViewModels.Car;
using RentACar.ViewModels.Rent;
using RentACar.ViewModels.User;

namespace RentACar.Services.Implementations
{
    public class RentService : IRentService
    {
        private readonly ICarService _carService;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public RentService(ICarService carService, UserManager<User> userManager, ApplicationDbContext context)
        {
            _carService = carService;
            _userManager = userManager;
            _context = context;
        }

        public async Task ConfirmRent(CreateRentViewModel viewModel)
        {
            Rent rent = new Rent
            {
                CarId = viewModel.Car.CarId,
                UserId = viewModel.User.UserId,
                DateOfRent = viewModel.DateOfRent.ToDateTime(TimeOnly.MinValue),
                DateOfReturn = viewModel.DateOfReturn.ToDateTime(TimeOnly.MinValue)
            };
            await _context.Rents.AddAsync(rent);
            await _context.SaveChangesAsync();
        }

        public async Task<CreateRentViewModel> CreatingRentAction(int carId, string userId)
        {
            var car = await _carService.GetById(carId);
            var user = await _userManager.FindByIdAsync(userId);

            if (car == null || user == null)
            {
                return null; 
            }

            var rents = await GetRentsByCarId(carId);

            var userViewModel = new UserViewModel
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName
            };

            var viewModel = new CreateRentViewModel
            {
                Car = car,
                User = userViewModel,
                DateOfRent = DateOnly.FromDateTime(DateTime.Now),
                DateOfReturn = DateOnly.FromDateTime(DateTime.Now.AddDays(7)), 
                TotalPrice = car.PricePerDay * 7, 
                Rents = rents
            };

            return viewModel;
        }

        private async Task<List<RentViewModel>> GetRentsByCarId(int carId)
        {
            var rents = await _context.Rents
                .Where(r => r.CarId == carId)
                .Include(r => r.User)
                .Include(r => r.Car)
                .ToListAsync();

            return rents.Select(rent => new RentViewModel
            {
                RentId = rent.RentId,
                User = new UserViewModel
                {
                    UserId = rent.User.Id,
                    FirstName = rent.User.FirstName,
                    MiddleName = rent.User.MiddleName,
                    LastName = rent.User.LastName
                },
                Car = new CarViewModel
                {
                    CarId = rent.Car.CarId,
                    Brand = rent.Car.Brand,
                    Model = rent.Car.Model,
                    LicensePlateNumber = rent.Car.LicensePlateNumber,
                    YearOfManufacture = rent.Car.YearOfProduction,
                    IsRented = rent.Car.IsRented,
                    PricePerDay = rent.Car.PricePerDay
                },
                DateOfRent = rent.DateOfRent,
                DateOfReturn = rent.DateOfReturn
            }).ToList();
        }
    }
}
