namespace RentACar.Models
{
    public class Car
    {
        public int CarId { get; set; }
        public string LicensePlateNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public DateOnly YearOfProduction { get; set; }
        public bool IsRented { get; set; }
        public double PricePerDay { get; set; }
    }
}
