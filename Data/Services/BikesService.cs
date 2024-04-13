using Microsoft.EntityFrameworkCore;
using RentABikeWebApp.Data.Base;
using RentABikeWebApp.Models;

namespace RentABikeWebApp.Data.Services
{
    public class BikesService(ApplicationDbContext context) : EntityBaseRepository<Bike>(context), IBikesService
    {
        public void UpdateBikeStatusBasedOnReservations(Bike bike)
        {
            if (bike.Status != StatusType.Broken)
            {
                DateTime currentDate = DateTime.Now;
                bool isActiveReservation = context.Bikes
                    .Include(b => b.Reservations)
                    .Where(b => b.Id == bike.Id)
                    .SelectMany(b => b.Reservations)
                    .Any(r => r.StartDate <= currentDate && r.EndDate >= currentDate);

                if (isActiveReservation)
                {
                    bike.Status = StatusType.Unavailable;
                }
                else
                {
                    bike.Status = StatusType.Available;
                }

                context.Update(bike);
                context.SaveChanges();
            }
        }

    }
}
