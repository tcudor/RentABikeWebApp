using RentABikeWebApp.Data.Base;
using RentABikeWebApp.Models;

namespace RentABikeWebApp.Data.Services
{
    public class BikesService : EntityBaseRepository<Bike>, IBikesService
    {
        public BikesService(ApplicationDbContext context) : base(context){ }
    }
}
