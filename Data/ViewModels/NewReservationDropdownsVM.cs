using RentABikeWebApp.Controllers;
using RentABikeWebApp.Models;

namespace RentABikeWebApp.Data.ViewModels
{
    public class NewReservationDropdownsVM
    {
        public NewReservationDropdownsVM() {
            Bikes = new List<Bike>();
            Customers = new List<Customer>();
        }

        public List<Bike> Bikes { get; set; }
        public List<Customer> Customers { get; set; }
    }
}
