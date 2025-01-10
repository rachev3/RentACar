using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace RentACar.Models
{
    public class Rent
    {
        public int RentId { get; set; }


        [ForeignKey("Car")]
        public int CarId { get; set; }

        public Car Car { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public User User { get; set; }

        public DateTime DateOfRent { get; set; }
        public DateTime DateOfReturn { get; set; }
        public double TotalPrice { get; set; }
    }
}
