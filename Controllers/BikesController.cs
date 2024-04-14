using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentABikeWebApp.Data;
using RentABikeWebApp.Data.Services;
using RentABikeWebApp.Models;

namespace RentABikeWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BikesController : Controller
    {
        private readonly IBikesService _service;

        public BikesController(IBikesService service)
        {
            _service = service;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Home()
        {
            var allBikes = await _service.GetAllAsync();
            foreach (var bike in allBikes)
            {
                _service.UpdateBikeStatusBasedOnReservations(bike);
            }
            return View(allBikes);
        }

        public async Task<IActionResult> Index()
        {
            var allBikes = await _service.GetAllAsync();
            foreach (var bike in allBikes)
            {
                _service.UpdateBikeStatusBasedOnReservations(bike);
            }
            return View(allBikes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Bike Bike, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(Bike);
            }
            if (imageFile != null && imageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(memoryStream);
                    Bike.Image = memoryStream.ToArray();
                }
            }
            await _service.AddAsync(Bike);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int Id)
        {
            var BikeDetails = await _service.GetByIdAsync(Id);

            if (BikeDetails == null)
            {
                return View("NotFound");
            }

            return View(BikeDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int Id, Bike Bike, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(Bike);
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(memoryStream);
                    Bike.Image = memoryStream.ToArray();
                }
            }
            else
            {
                var existingBike = await _service.GetByIdAsync(Id);
                if (existingBike != null)
                {
                    Bike.Image = existingBike.Image;
                }
            }

            await _service.UpdateAsync(Id, Bike);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Details(int id)
        {
            var bikeDetails = await _service.GetByIdAsync(id);
            if (bikeDetails == null)
            {
                return View("NotFound");
            }
            _service.UpdateBikeStatusBasedOnReservations(bikeDetails);

            return View(bikeDetails);
        }


        public async Task<IActionResult> Delete(int Id)
        {
            var BikeDetails = await _service.GetByIdAsync(Id);

            if (BikeDetails == null)
            {
                return View("NotFound");
            }

            return View(BikeDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int Id)
        {
            await _service.DeleteAsync(Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
