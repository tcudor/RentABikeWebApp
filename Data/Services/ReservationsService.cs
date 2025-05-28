using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
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


        public async Task<ReservationFormVM> GetReservationFormValuesAsync(int selectedBikeId, string? currentUserId, bool isAdmin, int? excludeReservationId = null)
        {
            var bikes = await _context.Bikes.OrderBy(b => b.Id).ToListAsync();
            if (!bikes.Any())
                throw new InvalidOperationException("No bikes available in the system.");

            if (!bikes.Any(b => b.Id == selectedBikeId))
            {
                selectedBikeId = bikes.First().Id;
            }

            List<Customer> customers = isAdmin
                ? await _context.Customers.OrderBy(c => c.Name).ToListAsync()
                : await _context.Customers
                    .Where(c => c.UserId == currentUserId)
                    .ToListAsync();

            var bikeItems = bikes
                .Select(b => new SelectListItem
                {
                    Text = $"Bike {b.Id} - {b.Type}",
                    Value = b.Id.ToString(),
                    Selected = b.Id == selectedBikeId
                })
                .ToList();

            var customerItems = customers
                .Select(c => new SelectListItem
                {
                    Text = c.Name ?? "",
                    Value = c.Id.ToString(),
                    Selected = false
                })
                .ToList();

            var price = bikes.First(b => b.Id == selectedBikeId).PricePerHour;

            var active = await GetActiveReservationsForBikeAsync(
                selectedBikeId, excludeReservationId);

            return new ReservationFormVM
            {
                SelectedBikeId = selectedBikeId,
                SelectedCustomerId = customerItems.FirstOrDefault()?.Value is string v
                                        ? int.Parse(v)
                                        : (int?)null,
                Bikes = bikeItems,
                Customers = customerItems,
                PricePerHour = price,
                ActiveReservations = active,
                Reservation = new Reservation
                {
                    BikeId = selectedBikeId,
                    CustomerId = customerItems.FirstOrDefault() != null
                                  ? int.Parse(customerItems.First().Value)
                                  : 0
                }
            };
        }

        public async Task<IEnumerable<ActiveReservationDto>> GetActiveReservationsForBikeAsync(int bikeId,int? reservationIdToExclude = null)
        {
            var now = DateTime.Now;
            var query = _context.Reservations
                .Where(r => r.BikeId == bikeId && r.EndDate > now);

            if (reservationIdToExclude.HasValue)
                query = query.Where(r => r.Id != reservationIdToExclude.Value);

            return await query
                .OrderBy(r => r.StartDate)
                .Select(r => new ActiveReservationDto
                {
                    StartDate = r.StartDate.ToString("dd/MM/yyyy HH:mm"),
                    EndDate = r.EndDate.ToString("dd/MM/yyyy HH:mm")
                })
                .ToListAsync();
        }

        public async Task<bool> IsBikeAvailableAsync(int bikeId, DateTime desiredStart, DateTime desiredEnd, int? reservationIdToExclude = null)
        {
            var overlaps = _context.Reservations
                .Where(r => r.BikeId == bikeId
                         && r.StartDate < desiredEnd
                         && r.EndDate > desiredStart);

            if (reservationIdToExclude.HasValue)
                overlaps = overlaps.Where(r => r.Id != reservationIdToExclude);

            return !await overlaps.AnyAsync();
        }

        public async Task<decimal> GetBikePriceAsync(int bikeId)
        {
            var bike = await _context.Bikes.FindAsync(bikeId)
                       ?? throw new KeyNotFoundException($"Bike {bikeId} not found");
            return bike.PricePerHour;
        }
    }
}
