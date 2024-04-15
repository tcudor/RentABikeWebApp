using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RentABikeWebApp.Data;
using RentABikeWebApp.Data.Services;
using RentABikeWebApp.Models;

namespace RentABikeWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReservationsController(IReservationsService service) : Controller
    {
        private readonly IReservationsService _service = service;

        public async Task<IActionResult> Index()
        {
            var allReservations = await _service.GetAllAsync();
            return View(allReservations);
        }

        public async Task<IActionResult> Create(int BikeId)
        {
            var reservationDropdownData = await _service.GetNewReservationDropdownsValues();
            ViewBag.Bikes = new SelectList(reservationDropdownData.Bikes.Select(b => new SelectListItem
            {
                Text = $"Bike {b.Id} - {b.Type}",
                Value = b.Id.ToString(),
                Selected = (b.Id == BikeId)
            }), "Value", "Text");

            var selectedBike = reservationDropdownData.Bikes.FirstOrDefault(b => b.Id == BikeId);
            ViewBag.PricePerHour = selectedBike != null ? selectedBike.PricePerHour : 0;

            ViewBag.Customers = new SelectList(reservationDropdownData.Customers, "Id", "Name");

            ViewBag.ActiveReservations = await _service.GetActiveReservationsForBikeAsync(BikeId);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Reservation Reservation)
        {
            if (!ModelState.IsValid)
            {
                var reservationDropdownData = await _service.GetNewReservationDropdownsValues();
                ViewBag.Bikes = new SelectList(reservationDropdownData.Bikes.Select(b => new SelectListItem
                {
                    Text = $"Bike {b.Id} - {b.Type}",
                    Value = b.Id.ToString()
                }), "Value", "Text");
                ViewBag.Customers = new SelectList(reservationDropdownData.Customers, "Id", "Name");
                return View(Reservation);
            }
            bool isBikeAvailable = await _service.IsBikeAvailableAsync(Reservation.BikeId, Reservation.StartDate, Reservation.EndDate);
            if (!isBikeAvailable)
            {
                var reservationDropdownData = await _service.GetNewReservationDropdownsValues();
                ModelState.AddModelError("BikeAvailability", "The selected bike is not available for the specified dates.");
                ViewBag.Bikes = new SelectList(reservationDropdownData.Bikes.Select(b => new SelectListItem
                {
                    Text = $"Bike {b.Id} - {b.Type}",
                    Value = b.Id.ToString()
                }), "Value", "Text");
                ViewBag.Customers = new SelectList(reservationDropdownData.Customers, "Id", "Name");
                return View(Reservation);
            }

            await _service.AddAsync(Reservation);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var ReservationDetails = await _service.GetByIdAsync(id);

            if (ReservationDetails == null)
            {
                return View("NotFound");
            }

            return View(ReservationDetails);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, Reservation Reservation)
        {
            if (!ModelState.IsValid)
            {
                var reservationDropdownData = await _service.GetNewReservationDropdownsValues();
                ViewBag.Bikes = new SelectList(reservationDropdownData.Bikes.Select(b => new SelectListItem
                {
                    Text = $"Bike {b.Id} - {b.Type}",
                    Value = b.Id.ToString()
                }), "Value", "Text"); ViewBag.Customers = new SelectList(reservationDropdownData.Customers, "Id", "Name");
                return View(Reservation);
            }

            bool isBikeAvailable = await _service.IsBikeAvailableAsync(Reservation.BikeId, Reservation.StartDate, Reservation.EndDate, Reservation.Id);
            if (!isBikeAvailable)
            {
                var reservationDropdownData = await _service.GetNewReservationDropdownsValues();
                ModelState.AddModelError("BikeAvailability", "The selected bike is not available for the specified dates.");
                ViewBag.Bikes = new SelectList(reservationDropdownData.Bikes.Select(b => new SelectListItem
                {
                    Text = $"Bike {b.Id} - {b.Type}",
                    Value = b.Id.ToString()
                }), "Value", "Text");
                ViewBag.Customers = new SelectList(reservationDropdownData.Customers, "Id", "Name");
                return View(Reservation);
            }

            await _service.UpdateAsync(id, Reservation);
            return RedirectToAction(nameof(Index));
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

        [HttpGet]
        public async Task<IActionResult> GetDataAsync(int BikeId)
        {
            var reservationDropdownData = await _service.GetNewReservationDropdownsValues();
            var bike = reservationDropdownData.Bikes.FirstOrDefault(b => b.Id == BikeId);
            if (bike != null)
            {
                var pricePerHour = bike.PricePerHour;
                var activeReservations = await _service.GetActiveReservationsForBikeAsync(BikeId);

                return Json(new { pricePerHour, activeReservations });
            }
            return BadRequest("Bike not found");
        }

    }
}
