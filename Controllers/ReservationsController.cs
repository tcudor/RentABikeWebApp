using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RentABikeWebApp.Data;
using RentABikeWebApp.Data.Services;
using RentABikeWebApp.Data.ViewModels;
using RentABikeWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RentABikeWebApp.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly IReservationsService _service;
        private readonly ICustomersService _customersService;
        private readonly UserManager<IdentityUser> _userManager;

        public ReservationsController(
            IReservationsService service,
            ICustomersService customersService,
            UserManager<IdentityUser> userManager)
        {
            _service = service;
            _customersService = customersService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var allReservations = await _service.GetAllAsync();
            return View(allReservations);
        }

        public async Task<IActionResult> Create(int? BikeId)
        {
            var vm = await _service.GetReservationFormValuesAsync(
                    BikeId.GetValueOrDefault(0),
                    _userManager.GetUserId(User),
                    User.IsInRole("Admin"));
            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> Create(ReservationFormVM vm)
        {
            if (User.IsInRole("Client"))
            {
                vm.Reservation.CustomerId =
                    (await _customersService.GetByUserIdAsync(
                        _userManager.GetUserId(User)))!.Id;
            }
            var available = await _service.IsBikeAvailableAsync(
                vm.Reservation.BikeId,
                vm.Reservation.StartDate,
                vm.Reservation.EndDate);

            if (!available)
            {
                ModelState.AddModelError(
                    "ReservationOverlap",
                    "These dates conflict with an existing reservation. " +
                    "Please pick a time range that does not overlap.");
            }

            if (!ModelState.IsValid)
            {
                var reload = await _service.GetReservationFormValuesAsync(
                    vm.Reservation.BikeId,
                    _userManager.GetUserId(User),
                    User.IsInRole("Admin"));
                reload.Reservation = vm.Reservation;
                return View(reload);
            }

            await _service.AddAsync(vm.Reservation);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return View("NotFound");

            var vm = await _service.GetReservationFormValuesAsync(
                existing.BikeId,
                _userManager.GetUserId(User),
                User.IsInRole("Admin"),
                excludeReservationId: id);

            vm.Reservation = existing;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ReservationFormVM vm)
        {
            if (User.IsInRole("Client"))
            {
                vm.Reservation.CustomerId =
                    (await _customersService.GetByUserIdAsync(
                        _userManager.GetUserId(User)))!.Id;
            }

            var available = await _service.IsBikeAvailableAsync(
                vm.Reservation.BikeId,
                vm.Reservation.StartDate,
                vm.Reservation.EndDate,
                id);

            if (!available)
            {
                ModelState.AddModelError(
                    "ReservationOverlap",
                    "These dates conflict with an existing reservation. " +
                    "Please pick a time range that does not overlap.");
            }

            if (!ModelState.IsValid)
            {
                var reload = await _service.GetReservationFormValuesAsync(
                    vm.Reservation.BikeId,
                    _userManager.GetUserId(User),
                    User.IsInRole("Admin"),
                    excludeReservationId: id);
                reload.Reservation = vm.Reservation;
                return View(reload);
            }

            await _service.UpdateAsync(id, vm.Reservation);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetData(int BikeId)
        {
            var price = await _service.GetBikePriceAsync(BikeId);
            var active = await _service.GetActiveReservationsForBikeAsync(BikeId);
            return Json(new
            {
                pricePerHour = price,
                activeReservations = active
            });

        }

        public async Task<IActionResult> Details(int id)
        {
            var ReservationDetails = await _service.GetByIdAsync(id);

            if (ReservationDetails == null)
            {
                return View("NotFound");
            }

            return View(ReservationDetails);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var ReservationDetails = await _service.GetByIdAsync(id);

            if (ReservationDetails == null)
            {
                return View("NotFound");
            }

            return View(ReservationDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
