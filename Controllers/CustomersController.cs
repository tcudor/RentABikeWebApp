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
    public class CustomersController : Controller
    {
        private readonly ICustomersService _service;

        public CustomersController(ICustomersService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var allCustomers = await _service.GetAllAsync();
            return View(allCustomers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Customer Customer)
        {
            if (!ModelState.IsValid)
            {
                return View(Customer);
            }

            await _service.AddAsync(Customer);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var CustomerDetails = await _service.GetByIdAsync(id);

            if (CustomerDetails == null)
            {
                return View("NotFound");
            }

            return View(CustomerDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Customer Customer)
        {
            if (!ModelState.IsValid)
            {
                return View(Customer);
            }

            await _service.UpdateAsync(id, Customer);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var CustomerDetails = await _service.GetByIdAsync(id);

            if (CustomerDetails == null)
            {
                return View("NotFound");
            }

            return View(CustomerDetails);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var CustomerDetails = await _service.GetByIdAsync(id);

            if (CustomerDetails == null)
            {
                return View("NotFound");
            }

            return View(CustomerDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
