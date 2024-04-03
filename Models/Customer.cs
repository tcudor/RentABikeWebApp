using System.ComponentModel.DataAnnotations;

namespace RentABikeWebApp.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? IdCode { get; set; }
        public string? IdSeries { get; set; }
        public List<Reservation>? Reservations { get; set; }
    }
}
