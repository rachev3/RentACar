using RentACar.ViewModels.Car;

namespace RentACar.Services.Interfaces
{
    public interface ICarService
    {
        Task<CarListViewModel> GetAll();
        Task<CarViewModel> GetById(int carId);
        Task Update(CarViewModel car);
        Task Create(CarViewModel car);
        Task Delete(int carId);
    }
}
