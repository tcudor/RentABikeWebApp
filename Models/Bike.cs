using RentABikeWebApp.Data;
using System.ComponentModel.DataAnnotations;

namespace RentABikeWebApp.Models
{
    public class Bike
    {
        [Key]
        public int BikeId { get; set; }
        public BikeType? Type { get; set; }
        public decimal PricePerHour { get; set; }
        public string? Status { get; set; }
        public byte[]? Image { get; set; } 
        public List<Reservation>? Reservations { get; set; }
    }
}
