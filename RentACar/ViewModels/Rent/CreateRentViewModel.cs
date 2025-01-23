using RentACar.ViewModels.Car;
using RentACar.ViewModels.User;

namespace RentACar.ViewModels.Rent
{
    public class CreateRentViewModel
    {
        public UserViewModel User { get; set; }
        public CarViewModel Car { get; set; }
        public DateOnly DateOfRent { get; set; }
        public DateOnly DateOfReturn { get; set; }
        public double TotalPrice { get; set; }
        public List<RentViewModel> Rents { get; set; }

        public List<string> RentedDays
        {
            get
            {
                var rentedDays = new List<string>();
                if (Rents != null)
                {
                    foreach (var rent in Rents)
                    {
                        for (var date = rent.DateOfRent.Date; date <= rent.DateOfReturn.Date; date = date.AddDays(1))
                        {
                            rentedDays.Add(date.ToString("yyyy-MM-dd"));
                        }
                    }
                }
                return rentedDays;
            }
        }
    }
}
