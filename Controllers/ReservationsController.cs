using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RentABikeWebApp.Data;
using RentABikeWebApp.Data.Services;
using RentABikeWebApp.Models;

namespace RentABikeWebApp.Controllers
{
    public class ReservationsController(IReservationsService service) : Controller
    {
        private readonly IReservationsService _service = service;

        public async Task<IActionResult> Index()
        {
            var allReservations = await _service.GetAllAsync();
            return View(allReservations);
        }

        public async Task<IActionResult> Create()
        {
            var reservationDropdownData=await _service.GetNewReservationDropdownsValues();
            ViewBag.Bikes = new SelectList(reservationDropdownData.Bikes.Select(b => new SelectListItem
            {
                Text = $"Bike {b.Id} - {b.Type}",
                Value = b.Id.ToString()
            }), "Value", "Text");
            ViewBag.Customers = new SelectList(reservationDropdownData.Customers, "Id", "Name");
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
                ModelState.AddModelError(string.Empty, "Bike is not available for the selected dates.");
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

            var response = new Reservation()
            {
                Id = ReservationDetails.Id,
                StartDate = ReservationDetails.StartDate,
                EndDate = ReservationDetails.EndDate,
                TotalCost = ReservationDetails.TotalCost,
                BikeId = ReservationDetails.BikeId,
                CustomerId = ReservationDetails.CustomerId,
            };

                var reservationDropdownData = await _service.GetNewReservationDropdownsValues();
                ViewBag.Bikes = new SelectList(reservationDropdownData.Bikes.Select(b => new SelectListItem
                {
                    Text = $"Bike {b.Id} - {b.Type}",
                    Value = b.Id.ToString()
                }), "Value", "Text"); ViewBag.Customers = new SelectList(reservationDropdownData.Customers, "Id", "Name");
            
            return View(response);
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

            bool isBikeAvailable = await _service.IsBikeAvailableAsync(Reservation.BikeId, Reservation.StartDate, Reservation.EndDate);
            if (!isBikeAvailable)
            {
                var reservationDropdownData = await _service.GetNewReservationDropdownsValues();
                ModelState.AddModelError(string.Empty, "Bike is not available for the selected dates.");
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
    }
}
