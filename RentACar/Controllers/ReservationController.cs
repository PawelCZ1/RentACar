using System.Net;
using AutoMapper;
using Investments.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACar.Data;
using RentACar.Models.Db;
using RentACar.Models.Dto;
using RentACar.Utils;

namespace RentACar.Controllers;
[Route("api/reservation")]
[ApiController]
public class ReservationController : ControllerBase
{
    private APIResponse _response;
    private readonly APIDatabaseContext _db;
    private readonly IMapper _mapper;

    public ReservationController(APIDatabaseContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetAllReservations()
    {
        var response = await _db.ReservationEntities.ToListAsync();
        var mappedResponse = _mapper.Map<List<ReservationDTO>>(response);
        _response = new APIResponse(HttpStatusCode.OK, true, null, mappedResponse);
        return Ok(_response);
    }
    
    [HttpGet]
    [Route("current")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetCurrentReservations()
    {
        var response = await _db.ReservationEntities.Where(E => E.ReservationFinishDate.CompareTo(DateTime.UtcNow) >= 0)
            .ToListAsync();
        var mappedResponse = _mapper.Map<List<ReservationDTO>>(response);
        _response = new APIResponse(HttpStatusCode.OK, true, null, mappedResponse);
        return Ok(_response);
    }
    
    [HttpGet("{id:int}", Name = "GetReservation")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> GetReservation(int id)
    {
        if (id == 0)
        {
            _response = new APIResponse(HttpStatusCode.BadRequest, false,
                new List<string> { "Id cannot equal 0" }, null);
            return BadRequest(_response);
        }

        var response = await _db.ReservationEntities.FirstOrDefaultAsync(e => e.Id == id);
        if (response == null)
        {
            _response = new APIResponse(HttpStatusCode.NotFound, false,
                new List<string> { "Reservation with id: " + id + " is not registered" }, null);
            return NotFound(_response);
        }

        var mappedResponse = _mapper.Map<ReservationDTO>(response);
        _response = new APIResponse(HttpStatusCode.OK, true, null, mappedResponse);
        return Ok(_response);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> RegisterReservation([FromBody] RegisterReservationDTO registerReservationDTO)
    {
        if (registerReservationDTO == null)
        {
            _response = new APIResponse(HttpStatusCode.BadRequest, false,
                new List<string> { "Provided registration data is incorrect" }, null);
            return BadRequest(_response);
        }
        
        if (registerReservationDTO.CustomerId <= 0 || registerReservationDTO.CarId <= 0)
        {
            _response = new APIResponse(HttpStatusCode.BadRequest, false,
                new List<string> { "Id cannot be equal or lesser than 0" }, null);
            return BadRequest(_response);
        }

        if (registerReservationDTO.Days <= 0)
        {
            _response = new APIResponse(HttpStatusCode.BadRequest, false,
                new List<string> { "Days amount must be positive" }, null);
            return BadRequest(_response);
        }

        var customerEntity = await _db.CustomerEntities.FirstOrDefaultAsync(e => e.Id == registerReservationDTO.CustomerId);
        if (customerEntity == null)
        {
            _response = new APIResponse(HttpStatusCode.NotFound, false,
                new List<string> { "Customer with id: " + registerReservationDTO.CustomerId + " is not registered" }, null);
            return NotFound(_response);
        }
        
        var carEntity = await _db.CarEntities.FirstOrDefaultAsync(e => e.Id == registerReservationDTO.CarId);
        if (carEntity == null)
        {
            _response = new APIResponse(HttpStatusCode.NotFound, false,
                new List<string> { "Car with id: " + registerReservationDTO.CarId + " is not registered" }, null);
            return NotFound(_response);
        }

        if (!carEntity.Availability)
        {
            _response = new APIResponse(HttpStatusCode.BadRequest, false,
                new List<string> { "Car with id: " + registerReservationDTO.CarId + " is not available" }, null);
            return BadRequest(_response);
        }

        var balanceAfterReservation =
            CustomerBalanceCalculator.CalculateBalanceAfterReservation(customerEntity.Balance, carEntity.PricePerDay,
                registerReservationDTO.Days);
        if (balanceAfterReservation < 0)
        {
            _response = new APIResponse(HttpStatusCode.BadRequest, false,
                new List<string> { "Customer does not have enough money" }, null);
            return BadRequest(_response);
        }

        customerEntity.Balance = balanceAfterReservation;

        var reservationDTO = new ReservationDTO
        {
            Id = 0,
            CustomerEntityId = customerEntity.Id,
            CarEntityId = carEntity.Id,
            ReservationStartDate = DateTime.UtcNow,
            ReservationFinishDate = DateTime.UtcNow.AddDays(registerReservationDTO.Days)
        };
        
        var entity = _mapper.Map<ReservationEntity>(reservationDTO);

        await _db.ReservationEntities.AddAsync(entity);
        _db.CustomerEntities.Update(customerEntity);
        await _db.SaveChangesAsync();
        var lastId = await _db.ReservationEntities.MaxAsync(e => e.Id);
        reservationDTO.Id = lastId;
        _response = new APIResponse(HttpStatusCode.Created, true, null, reservationDTO);
        return CreatedAtRoute("GetReservation", new {id = lastId}, _response);
    }
    
}