namespace RentACar.ViewModels.Car
{
    public class CarListViewModel
    {
        public CarListViewModel()
        {
            PriceRangeStart = 0; // Set default min price to 0
            PriceRangeEnd = 1000; // Set default max price to 1000
        }

        public List<CarViewModel> Cars { get; set; }
        public string BrandFilter { get; set; }
        public string ModelFilter { get; set; }
        public double PriceRangeStart { get; set; }
        public double PriceRangeEnd { get; set; }
        public int? StartYearFilter { get; set; }
        public int? EndYearFilter { get; set; }
        public bool? IsRentedFilter { get; set; }
        public string ImageUrl { get; set; }
    }
}
