using RentABikeWebApp.Data.Base;
using RentABikeWebApp.Data.ViewModels;
using RentABikeWebApp.Models;

namespace RentABikeWebApp.Data.Services
{
    public interface IReservationsService : IEntityBaseRepository<Reservation>
    {
        Task<ReservationFormVM> GetReservationFormValuesAsync(
                int selectedBikeId,
                string? currentUserId,
                bool isAdmin,
                int? excludeReservationId = null);

        Task<IEnumerable<ActiveReservationDto>> GetActiveReservationsForBikeAsync(
            int bikeId,
            int? reservationIdToExclude = null);

        Task<bool> IsBikeAvailableAsync(
            int bikeId,
            DateTime startDate,
            DateTime endDate,
            int? reservationIdToExclude = null);

        Task<decimal> GetBikePriceAsync(int bikeId);
    }
}