using System.Net;
using AutoMapper;
using Investments.Models.Api;
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

    public CarController(ApiDatabaseContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetCars()
    {
        var response = await _db.CarEntities.ToListAsync();
        var mappedResponse = _mapper.Map<List<CarDTO>>(response);
        _response = new APIResponse(HttpStatusCode.OK, true, null, mappedResponse);
        return Ok(_response);
    }

    [HttpGet("{id:int}", Name = "GetCar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> GetCar(int id)
    {
        if (id == 0)
        {
            _response = new APIResponse(HttpStatusCode.BadRequest, false,
                new List<string> { "Id cannot equal 0" }, null);
            return BadRequest(_response);
        }

        var response = await _db.CarEntities.FirstOrDefaultAsync(e => e.Id == id);
        if (response == null)
        {
            _response = new APIResponse(HttpStatusCode.NotFound, false,
                new List<string> { "Car with id: " + id + " is not registered" }, null);
            return NotFound(_response);
        }

        var mappedResponse = _mapper.Map<CarDTO>(response);
        _response = new APIResponse(HttpStatusCode.OK, true, null, mappedResponse);
        return Ok(_response);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerDTO>> RegisterCar([FromBody] CarDTO carDTO)
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


}