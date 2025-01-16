using RentACar.ViewModels.Car;

namespace RentACar.Services.Interfaces
{
    public interface ICarService
    {
        Task<CarListViewModel> GetAll();
        Task Delete(int carId);
    }
}
