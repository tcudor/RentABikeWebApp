using RentABikeWebApp.Data.Base;
using RentABikeWebApp.Models;

namespace RentABikeWebApp.Data.Services
{
    public interface ICustomersService : IEntityBaseRepository<Customer>
    {
        Task<Customer?> GetByUserIdAsync(string userId);
    }
}