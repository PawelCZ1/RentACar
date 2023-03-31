using System.Net;
using AutoMapper;
using Investments.Models.Api;
using Investments.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACar.Data;
using RentACar.Models.Db;
using RentACar.Models.Dto;

namespace RentACar.Controllers;

[Route("api/car")]
[ApiController]
public class CarController : ControllerBase
{
    private APIResponse _response;
    private readonly ApiDatabaseContext _db;
    private readonly IMapper _mapper;
    private readonly ICarService _service;

    public CarController(ApiDatabaseContext db, IMapper mapper, ICarService service)
    {
        _db = db;
        _mapper = mapper;
        _service = service;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetCars()
    {
        var result = await _service.GetCars();
        _response = new APIResponse(HttpStatusCode.OK, true, null, result);
        return Ok(_response);
    }

    [HttpGet("{id:int}", Name = "GetCar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> GetCar(int id)
    {
        try
        {
            var result = await _service.GetCar(id);
            _response = new APIResponse(HttpStatusCode.OK, true, null, result);
            return Ok(_response);
        }
        catch (Exception e)
        {
            if (e.Message.Equals("Id cannot equal 0"))
            {
                _response = new APIResponse(HttpStatusCode.BadRequest, false,
                    new List<string> { e.Message }, null);
                return BadRequest(_response);
            }

            if (e.Message.Equals("Car with id: " + id + " is not registered"))
            {
                _response = new APIResponse(HttpStatusCode.NotFound, false,
                    new List<string> { e.Message }, null);
                return NotFound(_response);
            }
            
            _response = new APIResponse(HttpStatusCode.InternalServerError, false,
                new List<string> { e.Message }, null);
            return StatusCode(StatusCodes.Status500InternalServerError, _response);
        }
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> RegisterCar([FromBody] CarDTO carDTO)
    {
        
        if (carDTO == null || carDTO.Id != 0)
        {
            _response = new APIResponse(HttpStatusCode.BadRequest, false,
                new List<string> { "Provided registration data is incorrect" }, null);
            return BadRequest(_response);
        }
        
        var entity = _mapper.Map<CarEntity>(carDTO);

        await _db.CarEntities.AddAsync(entity);
        await _db.SaveChangesAsync();
        var lastId = await _db.CarEntities.MaxAsync(e => e.Id);
        carDTO.Id = lastId;
        _response = new APIResponse(HttpStatusCode.Created, true, null, carDTO);
        return CreatedAtRoute("GetCar", new {id = lastId}, _response);
    }
    
    [HttpDelete("{id:int}", Name = "DeleteCar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> DeleteCar(int id)
    {
        if (id <= 0)
        {
            _response = new APIResponse(HttpStatusCode.BadRequest, false,
                new List<string> { "Id cannot be equal or lesser than 0" }, null);
            return BadRequest(_response);
        }

        var entity = await _db.CarEntities.FirstOrDefaultAsync(e => e.Id == id);

        if (entity == null)
        {
            _response = new APIResponse(HttpStatusCode.NotFound, false,
                new List<string> { "Car with id: " + id + " is not registered" }, null);
            return NotFound(_response);
        }

        _db.CarEntities.Remove(entity);
        await _db.SaveChangesAsync();
        _response = new APIResponse(HttpStatusCode.OK, true,
            null, "Car with id: " + id + " was removed from database");
        return Ok(_response);
    }
    
    [HttpPut("{id:int}", Name = "UpdateCar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> UpdateCar(int id, [FromBody] CarDTO carDTO)
    {
        if (id != carDTO.Id)
        {
            _response = new APIResponse(HttpStatusCode.BadRequest, false,
                new List<string> { "DTO id does not match route id" }, null);
            return BadRequest(_response);
        }
        
        if (carDTO == null)
        {
            _response = new APIResponse(HttpStatusCode.BadRequest, false,
                new List<string> { "Provided registration data is incorrect" }, null);
            return BadRequest(_response);
        }
        
        var entity = await _db.CarEntities.FirstOrDefaultAsync(e => e.Id == id);

        if (entity == null)
        {
            _response = new APIResponse(HttpStatusCode.NotFound, false,
                new List<string> { "Car with id: " + id + " is not registered" }, null);
            return NotFound(_response);
        }

        var model = _mapper.Map<CarEntity>(carDTO);
        _db.CarEntities.Update(model);
        await _db.SaveChangesAsync();
        _response = new APIResponse(HttpStatusCode.OK, true,
            null, "Car with id: " + id + " was updated successfully");
        return Ok(_response);
    }


}