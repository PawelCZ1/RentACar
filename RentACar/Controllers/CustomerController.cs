using System.Net;
using AutoMapper;
using Investments.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACar.Data;
using RentACar.Models.Db;
using RentACar.Models.Dto;

namespace RentACar.Controllers;

[Route("api/customer")]
[ApiController]
public class CustomerController : ControllerBase
{
    private APIResponse _response;
    private readonly ApiDatabaseContext _db;
    private readonly IMapper _mapper;

    public CustomerController(ApiDatabaseContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
        
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetCustomers()
    {
        var response = await _db.CustomerEntities.ToListAsync();
        var mappedResponse = _mapper.Map<List<CustomerDTO>>(response);
        _response = new APIResponse(HttpStatusCode.OK, true, null, mappedResponse);
        return Ok(_response);
    }

    [HttpGet("{id:int}", Name = "GetCustomer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> GetCustomer(int id)
    {
        if (id == 0)
        {
            //ModelState.AddModelError("WrongId","Id cannot equal 0");
            _response = new APIResponse(HttpStatusCode.BadRequest, false,
                new List<string> { "Id cannot equal 0" }, null);
            return BadRequest(_response);
        }

        var response = await _db.CustomerEntities.FirstOrDefaultAsync(e => e.Id == id);
        if (response == null)
        {
            //ModelState.AddModelError("CustomerNotRegistered","Customer with id: " + id + " is not registered");
            _response = new APIResponse(HttpStatusCode.NotFound, false,
                new List<string> { "Customer with id: " + id + " is not registered" }, null);
            return NotFound(_response);
        }

        var mappedResponse = _mapper.Map<CustomerDTO>(response);
        _response = new APIResponse(HttpStatusCode.OK, true, null, mappedResponse);
        return Ok(_response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerDTO>> RegisterCustomer([FromBody] CustomerDTO customerDTO)
    {
        if (await _db.CustomerEntities.FirstOrDefaultAsync(e => e.Name == customerDTO.Name && e.Surname == customerDTO.Surname) != null)
        {
            //ModelState.AddModelError("CustomerAlreadyRegistered","This customer is already registered");
            _response = new APIResponse(HttpStatusCode.BadRequest, false,
                new List<string> { "This customer is already registered" }, null);
            return BadRequest(_response);
        }

        if (customerDTO == null || customerDTO.Id != 0)
        {
            //ModelState.AddModelError("IncorrectRegistrationData", "Provided registration data is incorrect");
            _response = new APIResponse(HttpStatusCode.BadRequest, false,
                new List<string> { "Provided registration data is incorrect" }, null);
            return BadRequest(_response);
        }
        
        var entity = _mapper.Map<CustomerEntity>(customerDTO);

        await _db.CustomerEntities.AddAsync(entity);
        await _db.SaveChangesAsync();
        var lastId = await _db.CustomerEntities.MaxAsync(e => e.Id);
        customerDTO.Id = lastId;
        _response = new APIResponse(HttpStatusCode.Created, true, null, customerDTO);
        return CreatedAtRoute("GetCustomer", new {id = lastId}, _response);
    }

    [HttpDelete("{id:int}", Name = "DeleteCustomer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> DeleteCustomer(int id)
    {
        if (id <= 0)
        {
            _response = new APIResponse(HttpStatusCode.BadRequest, false,
                new List<string> { "Id cannot be equal or lesser than 0" }, null);
            return BadRequest(_response);
        }

        var entity = await _db.CustomerEntities.FirstOrDefaultAsync(e => e.Id == id);

        if (entity == null)
        {
            _response = new APIResponse(HttpStatusCode.NotFound, false,
                new List<string> { "Customer with id: " + id + " is not registered" }, null);
            return NotFound(_response);
        }

        _db.CustomerEntities.Remove(entity);
        await _db.SaveChangesAsync();
        _response = new APIResponse(HttpStatusCode.OK, true,
            null, "Customer with id: " + id + " was removed from database");
        return Ok(_response);
    }  

}