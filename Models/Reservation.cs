using System.ComponentModel.DataAnnotations;

namespace RentABikeWebApp.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalCost { get; set; }

        public int BikeId { get; set; }
        public Bike? Bike { get; set; }

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

    }
}
