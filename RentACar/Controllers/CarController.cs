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
    private readonly ICarService _service;

    public CarController(APIDatabaseContext db, IMapper mapper, ICarService service)
    {
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
        try
        {
            var result = await _service.RegisterCar(carDTO);
            _response = new APIResponse(HttpStatusCode.Created, true, null, result);
            return CreatedAtRoute("GetCar", new {id = result.Id}, _response);
        }
        catch (Exception e)
        {
            if (e.Message.Equals("Provided registration data is incorrect"))
            {
                _response = new APIResponse(HttpStatusCode.BadRequest, false,
                    new List<string> { e.Message }, null);
                return BadRequest(_response);
            }
            
            _response = new APIResponse(HttpStatusCode.InternalServerError, false,
                new List<string> { e.Message }, null);
            return StatusCode(StatusCodes.Status500InternalServerError, _response);
        }
    }
    
    [HttpDelete("{id:int}", Name = "DeleteCar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> DeleteCar(int id)
    {
        try
        {
            var result = await _service.DeleteCar(id);
            _response = new APIResponse(HttpStatusCode.OK, true,
                null, result);
            return Ok(_response);
        }
        catch (Exception e)
        {
            if (e.Message.Equals("Id cannot be equal or lesser than 0"))
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
    
    [HttpPut("{id:int}", Name = "UpdateCar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> UpdateCar(int id, [FromBody] CarDTO carDTO)
    {
        try
        {
            var result = await _service.UpdateCar(id, carDTO);
            _response = new APIResponse(HttpStatusCode.OK, true,
            null, result);
            return Ok(_response);
        }
        catch(Exception e)
        {
            if(e.Message.Equals("DTO id does not match route id"))
            {
                _response = new APIResponse(HttpStatusCode.BadRequest, false,
                new List<string> { e.Message }, null);
                return BadRequest(_response);
            }

            if(e.Message.Equals("Provided registration data is incorrect"))
            {
                _response = new APIResponse(HttpStatusCode.BadRequest, false,
                new List<string> { e.Message }, null);
                return BadRequest(_response);
            }

            if(e.Message.Equals("Car with id: " + id + " is not registered"))
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


}