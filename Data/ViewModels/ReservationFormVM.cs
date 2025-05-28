using Microsoft.AspNetCore.Mvc.Rendering;
using RentABikeWebApp.Controllers;
using RentABikeWebApp.Models;

namespace RentABikeWebApp.Data.ViewModels
{
    public class ActiveReservationDto
    {
        public string StartDate { get; set; } = "";
        public string EndDate { get; set; } = "";
    }

    public class ReservationFormVM
    {
        public int SelectedBikeId { get; set; }
        public int? SelectedCustomerId { get; set; }
        public List<SelectListItem> Bikes { get; set; } = new();
        public List<SelectListItem> Customers { get; set; } = new();
        public decimal PricePerHour { get; set; }
        public IEnumerable<ActiveReservationDto> ActiveReservations { get; set; } = new List<ActiveReservationDto>();
        public Reservation Reservation { get; set; } = new();
    }
}
