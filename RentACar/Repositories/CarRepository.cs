using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RentACar.Data;
using RentACar.Models.Db;

namespace Investments.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly APIDatabaseContext _db;

        public CarRepository(APIDatabaseContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<CarEntity>> GetCars()
        {
            var result = await _db.CarEntities.ToListAsync();
            return result;
        }

        public async Task<CarEntity?> GetCar(int id)
        {
            var result = await _db.CarEntities.FirstOrDefaultAsync(e => e.Id == id);
            return result;
        }

        public async ValueTask<EntityEntry<CarEntity>> RegisterCar(CarEntity entity)
        {
            var result = await _db.CarEntities.AddAsync(entity);
            return result;
        }

        public async Task<int> SaveChanges()
        {
            var result = await _db.SaveChangesAsync();
            return result;
        }

        public async Task<int> LastCarId()
        {
            var result = await _db.CarEntities.MaxAsync(e => e.Id);
            return result;
        }

        public EntityEntry<CarEntity> RemoveCar(CarEntity entity)
        {
            var result = _db.CarEntities.Remove(entity);
            return result;
        }

        public EntityEntry<CarEntity> UpdateCar(CarEntity entity)
        {
            var result = _db.CarEntities.Update(entity);
            return result;
        }
    }
}
