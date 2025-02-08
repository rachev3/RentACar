namespace RentACar.ViewModels.Car
{
    public class CarViewModel
    {
        public int CarId { get; set; }
        public string LicensePlateNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public DateOnly YearOfManufacture { get; set; }
        public bool IsRented { get; set; }
        public double PricePerDay { get; set; }
        public string ImageUrl { get; set; }
    }
}
