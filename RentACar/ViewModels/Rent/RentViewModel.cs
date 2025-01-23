using RentACar.ViewModels.Car;
using RentACar.ViewModels.User;

namespace RentACar.ViewModels.Rent
{
    public class RentViewModel
    {
        public int RentId { get; set; }
        public UserViewModel User { get; set; }
        public CarViewModel Car { get; set; }
        public DateTime DateOfRent { get; set; }
        public DateTime DateOfReturn { get; set; }
    }
}
