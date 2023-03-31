using System.Net;
using AutoMapper;
using Investments.Models.Api;
using Microsoft.EntityFrameworkCore;
using RentACar.Data;
using RentACar.Models.Dto;

namespace Investments.Services;

public class CarService : ICarService
{
    private readonly ApiDatabaseContext _db;
    private readonly IMapper _mapper;

    public CarService(ApiDatabaseContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    public async Task<IEnumerable<CarDTO>> GetCars()
    {
        var result = await _db.CarEntities.ToListAsync();
        var mappedResult = _mapper.Map<List<CarDTO>>(result);
        return mappedResult;
    }

    public async Task<CarDTO> GetCar(int id)
    {
        if (id == 0)
        {
            throw new Exception("Id cannot equal 0");
        }

        var result = await _db.CarEntities.FirstOrDefaultAsync(e => e.Id == id);
        if (result == null)
        {
            throw new Exception("Car with id: " + id + " is not registered");
        }

        var mappedResult = _mapper.Map<CarDTO>(result);
        return mappedResult;
    }

    public async Task<CarDTO> RegisterCar(CarDTO carDTO)
    {
        throw new NotImplementedException();
    }
}