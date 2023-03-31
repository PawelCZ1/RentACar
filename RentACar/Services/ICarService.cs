using RentACar.Models.Dto;

namespace Investments.Services;

public interface ICarService
{
    Task<IEnumerable<CarDTO>> GetCars();

    Task<CarDTO> GetCar(int id);

    Task<CarDTO> RegisterCar(CarDTO carDTO);
}