using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentABikeWebApp.Data;
using RentABikeWebApp.Data.Services;
using RentABikeWebApp.Models;

namespace RentABikeWebApp.Controllers
{
    public class BikesController : Controller
    {
        private readonly IBikesService _service;

        public BikesController(IBikesService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var allBikes = await _service.GetAllAsync();
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

        public async Task<IActionResult> Edit(int id)
        {
            var BikeDetails = await _service.GetByIdAsync(id);

            if (BikeDetails == null)
            {
                return View("NotFound");
            }

            return View(BikeDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Bike Bike, IFormFile imageFile)
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

            await _service.UpdateAsync(id, Bike);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Details(int id)
        {
            var BikeDetails = await _service.GetByIdAsync(id);

            if (BikeDetails == null)
            {
                return View("NotFound");
            }

            return View(BikeDetails);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var BikeDetails = await _service.GetByIdAsync(id);

            if (BikeDetails == null)
            {
                return View("NotFound");
            }

            return View(BikeDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
