using RentABikeWebApp.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace RentABikeWebApp.Models
{
    public class Reservation : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public decimal TotalCost { get; set; }

        [Display(Name="Bike")]
        public int BikeId { get; set; }
        public Bike? Bike { get; set; }

        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

    }
}
