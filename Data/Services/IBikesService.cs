using RentABikeWebApp.Data.Base;
using RentABikeWebApp.Models;

namespace RentABikeWebApp.Data.Services
{
    public interface IBikesService : IEntityBaseRepository<Bike>
    {
    }
}