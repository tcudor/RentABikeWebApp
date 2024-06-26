﻿using RentABikeWebApp.Data.Base;
using RentABikeWebApp.Data.ViewModels;
using RentABikeWebApp.Models;

namespace RentABikeWebApp.Data.Services
{
    public interface IReservationsService : IEntityBaseRepository<Reservation>
    {
        Task<IEnumerable<object>> GetActiveReservationsForBikeAsync(int BikeId);
        Task<NewReservationDropdownsVM> GetNewReservationDropdownsValues();
        Task<bool> IsBikeAvailableAsync(int BikeId, DateTime StartDate, DateTime EndDate, int? reservationIdToExclude = null);
    }
}