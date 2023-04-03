using Microsoft.EntityFrameworkCore.ChangeTracking;
using RentACar.Models.Db;

namespace Investments.Repositories
{
    public interface ICarRepository
    {
        Task<IEnumerable<CarEntity>> GetCars();

        Task<CarEntity?> GetCar(int id);

        ValueTask<EntityEntry<CarEntity>> RegisterCar(CarEntity entity);

        Task<int> SaveChanges();

        Task<int> LastCarId();

        EntityEntry<CarEntity> RemoveCar(CarEntity entity);

        EntityEntry<CarEntity> UpdateCar(CarEntity entity);

    }
}
