using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentABikeWebApp.Models;
using RentABikeWebApp.Data.Services;

public class CustomersController : Controller
{
    private readonly ICustomersService _service;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public CustomersController(
        ICustomersService service,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _service = service;
        _userManager = userManager;
        _roleManager = roleManager;
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
    public async Task<IActionResult> Create(Customer customer)
    {
        if (!ModelState.IsValid)
            return View(customer);
        await _service.AddAsync(customer);
        if (!string.IsNullOrWhiteSpace(customer.Email))
        {
            var tempPassword = "Temp#1234";

            var user = new IdentityUser
            {
                UserName = customer.Email!,
                Email = customer.Email!
            };

            var createUserResult = await _userManager.CreateAsync(user, tempPassword);
            if (createUserResult.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("Client"))
                    await _roleManager.CreateAsync(new IdentityRole("Client"));

                await _userManager.AddToRoleAsync(user, "Client");
                customer.UserId = user.Id;
                await _service.UpdateAsync(customer.Id, customer);
            }
            else
            {
                foreach (var error in createUserResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return View(customer);
            }
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var customer = await _service.GetByIdAsync(id);
        if (customer == null) return View("NotFound");
        return View(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Customer customer)
    {
        if (!ModelState.IsValid)
            return View(customer);

        await _service.UpdateAsync(id, customer);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var customer = await _service.GetByIdAsync(id);
        if (customer == null) return View("NotFound");
        return View(customer);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var customer = await _service.GetByIdAsync(id);
        if (customer == null) return View("NotFound");
        return View(customer);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _service.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
