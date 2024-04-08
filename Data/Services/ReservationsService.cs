using Microsoft.EntityFrameworkCore;
using RentABikeWebApp.Data.Base;
using RentABikeWebApp.Data.ViewModels;
using RentABikeWebApp.Models;


namespace RentABikeWebApp.Data.Services
{
    public class ReservationsService : EntityBaseRepository<Reservation>, IReservationsService
    {
        private readonly ApplicationDbContext _context;
        public ReservationsService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            return await _context.Reservations
                .Include(b => b.Bike)
                .Include(b => b.Customer)
                .ToListAsync();
        }
        public async Task<NewReservationDropdownsVM> GetNewReservationDropdownsValues()
        {
            var response = new NewReservationDropdownsVM()
            {
                Bikes=await _context.Bikes.OrderBy(n => n.Id).ToListAsync(),
                Customers=await _context.Customers.OrderBy(n => n.Name).ToListAsync(),
            };
            return response;
        }
    }
}
