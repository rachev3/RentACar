using RentACar.ViewModels.Rent;

namespace RentACar.Services.Interfaces
{
    public interface IRentService
    {
        Task<CreateRentViewModel> CreatingRentAction(int carId, string userId);
        Task ConfirmRent(CreateRentViewModel viewModel);
    }
}
