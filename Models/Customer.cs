using RentABikeWebApp.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace RentABikeWebApp.Models
{
    public class Customer : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? IdCode { get; set; }
        public string? IdSeries { get; set; }
        public List<Reservation>? Reservations { get; set; }
    }
}
