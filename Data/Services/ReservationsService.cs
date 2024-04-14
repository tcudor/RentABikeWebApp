using Microsoft.EntityFrameworkCore;
using RentABikeWebApp.Data.Base;
using RentABikeWebApp.Data.ViewModels;
using RentABikeWebApp.Models;


namespace RentABikeWebApp.Data.Services
{
    public class ReservationsService(ApplicationDbContext context) : EntityBaseRepository<Reservation>(context), IReservationsService
    {
        private readonly ApplicationDbContext _context = context;

        public override async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            return await _context.Reservations
                .Include(b => b.Bike)
                .Include(b => b.Customer)
                .ToListAsync();
        }

        public override async Task<Reservation> GetByIdAsync(int id)
        {
            return await _context.Reservations
                .Include(b => b.Bike)
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(n => n.Id == id);
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

        public async Task<bool> IsBikeAvailableAsync(int BikeId, DateTime StartDate, DateTime EndDate, int? reservationIdToExclude = null)
        {
            IQueryable<Reservation> reservationsQuery = _context.Reservations
                .Where(r => r.BikeId == BikeId)
                .Where(r => (r.StartDate <= StartDate && r.EndDate >= StartDate) || (r.StartDate <= EndDate && r.EndDate >= EndDate));

            if (reservationIdToExclude.HasValue)
            {
                reservationsQuery = reservationsQuery.Where(r => r.Id != reservationIdToExclude);
            }

            return await reservationsQuery.CountAsync() == 0;
        }

        public async Task<IEnumerable<object>> GetActiveReservationsForBikeAsync(int BikeId)
        {
            var currentDate = DateTime.Now;
            var reservations = await _context.Reservations
                .Where(r => r.BikeId == BikeId && r.StartDate >= currentDate)
                .ToListAsync();

            return reservations.Select(r => new
            {
                StartDate = r.StartDate.ToString("dd/MM/yyyy HH:mm"),
                EndDate = r.EndDate.ToString("dd/MM/yyyy HH:mm")
            });
        }

    }
}
