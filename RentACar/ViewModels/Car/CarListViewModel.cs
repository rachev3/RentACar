namespace RentACar.ViewModels.Car
{
    public class CarListViewModel
    {
        public List<CarViewModel> Cars { get; set; } 
        public string BrandFilter { get; set; }
        public string ModelFilter { get; set; }
        public double PriceRangeStart { get; set; }
        public double PriceRangeEnd { get; set; }
        public DateOnly YearOfManufactureFilter { get; set; }
        public bool? IsRentedFilter { get; set; }
    }
}
