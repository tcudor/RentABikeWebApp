using RentABikeWebApp.Data;
using RentABikeWebApp.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace RentABikeWebApp.Models
{
    public class Bike : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        public BikeType? Type { get; set; }
        public decimal PricePerHour { get; set; }
        public string? Status { get; set; }
        public byte[]? Image { get; set; } 
        public List<Reservation>? Reservations { get; set; }
        public string DisplayName ()
        {
            return "Bike " + Id + " " + Type;
        }
    }
}
