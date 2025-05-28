using Microsoft.EntityFrameworkCore;
using RentABikeWebApp.Data.Base;
using RentABikeWebApp.Models;

namespace RentABikeWebApp.Data.Services
{
    public class CustomersService(ApplicationDbContext context) : EntityBaseRepository<Customer>(context), ICustomersService
    {
        private readonly ApplicationDbContext _context = context;

        public override async Task<IEnumerable<Customer>> GetAllAsync()
        => await _context.Customers
                         .Include(c => c.Reservations)
                         .ToListAsync();

        public override async Task<Customer> GetByIdAsync(int id)
            => await _context.Customers
                             .Include(c => c.Reservations)
                             .FirstOrDefaultAsync(c => c.Id == id);
        public async Task<Customer?> GetByUserIdAsync(string userId)
        => await _context.Customers
                         .Include(c => c.Reservations)
                         .FirstOrDefaultAsync(c => c.UserId == userId);
    }
}
