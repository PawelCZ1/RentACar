using System.Net;
using AutoMapper;
using Investments.Models.Api;
using Investments.Repositories;
using Microsoft.EntityFrameworkCore;
using RentACar.Data;
using RentACar.Models.Db;
using RentACar.Models.Dto;

namespace Investments.Services;

public class CarService : ICarService
{
    private readonly APIDatabaseContext _db;
    private readonly IMapper _mapper;
    private readonly ICarRepository _carRepository;

    public CarService(APIDatabaseContext db, IMapper mapper, ICarRepository carRepository)
    {
        _db = db;
        _mapper = mapper;
        _carRepository = carRepository;
    }
    public async Task<IEnumerable<CarDTO>> GetCars()
    {
        var result = await _carRepository.GetCars();
        var mappedResult = _mapper.Map<List<CarDTO>>(result);
        return mappedResult;
    }

    public async Task<CarDTO> GetCar(int id)
    {
        if (id == 0)
        {
            throw new Exception("Id cannot equal 0");
        }

        var result = await _carRepository.GetCar(id);
        if (result == null)
        {
            throw new Exception("Car with id: " + id + " is not registered");
        }

        var mappedResult = _mapper.Map<CarDTO>(result);
        return mappedResult;
    }

    public async Task<CarDTO> RegisterCar(CarDTO carDTO)
    {
        if (carDTO == null || carDTO.Id != 0)
        {
            throw new Exception("Provided registration data is incorrect");
        }
        
        var entity = _mapper.Map<CarEntity>(carDTO);
        await _carRepository.RegisterCar(entity);
        await _carRepository.SaveChanges();
        var lastId = await _carRepository.LastCarId();
        carDTO.Id = lastId;
        return carDTO;
    }

    public async Task<string> DeleteCar(int id)
    {
        if (id <= 0)
        {
            throw new Exception("Id cannot be equal or lesser than 0");
        }

        var entity = await _carRepository.GetCar(id);

        if (entity == null)
        {
            throw new Exception("Car with id: " + id + " is not registered");
        }

        _carRepository.RemoveCar(entity);
        await _carRepository.SaveChanges();
        return "Car with id: " + id + " was removed from database";
    }

    public async Task<string> UpdateCar(int id, CarDTO carDTO)
    {
        if (id != carDTO.Id)
        {
            throw new Exception("DTO id does not match route id");
        }
        
        if (carDTO == null)
        {
            throw new Exception("Provided registration data is incorrect");
        }
        
        var entity = await _carRepository.GetCar(id);

        if (entity == null)
        {
            throw new Exception("Car with id: " + id + " is not registered");
        }

        var model = _mapper.Map<CarEntity>(carDTO);
        _carRepository.UpdateCar(model);
        await _carRepository.SaveChanges();
        return "Car with id: " + id + " was updated successfully";
    }
}