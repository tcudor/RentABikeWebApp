using RentABikeWebApp.Data.Base;
using RentABikeWebApp.Models;

namespace RentABikeWebApp.Data.Services
{
    public class CustomersService : EntityBaseRepository<Customer>, ICustomersService
    {
        public CustomersService(ApplicationDbContext context) : base(context){ }
    }
}
