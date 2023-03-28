using AutoMapper;
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
    private readonly ApiDatabaseContext _db;
    private readonly IMapper _mapper;

    public CustomerController(ApiDatabaseContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomers()
    {
        var response = await _db.CustomerEntities.ToListAsync();
        var mappedResponse = _mapper.Map<List<CustomerDTO>>(response);
        return Ok(mappedResponse);
    }

    [HttpGet("{id:int}", Name = "GetCustomer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDTO>> GetCustomer(int id)
    {
        if (id == 0)
        {
            ModelState.AddModelError("WrongId","Id cannot equal 0");
            return BadRequest(ModelState["WrongId"]);
        }

        var response = await _db.CustomerEntities.FirstOrDefaultAsync(e => e.Id == id);
        if (response == null)
        {
            ModelState.AddModelError("CustomerNotRegistered","Customer with id: " + id + " is not registered");
            return NotFound(ModelState["CustomerNotRegistered"]);
        }

        var mappedResponse = _mapper.Map<CustomerDTO>(response);
        return Ok(mappedResponse);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CustomerDTO>> RegisterCustomer([FromBody] CustomerDTO customerDTO)
    {
        if (await _db.CustomerEntities.FirstOrDefaultAsync(e => e.Name == customerDTO.Name && e.Surname == customerDTO.Surname) != null)
        {
            ModelState.AddModelError("CustomerAlreadyRegistered","This customer is already registered");
            return BadRequest(ModelState["CustomerAlreadyRegistered"]);
        }

        if (customerDTO == null)
        {
            ModelState.AddModelError("IncorrectRegistrationData", "Provided registration data is incorrect");
            return BadRequest(ModelState["IncorrectRegistrationData"]);
        }

        var entity = _mapper.Map<CustomerEntity>(customerDTO);

        await _db.CustomerEntities.AddAsync(entity);
        await _db.SaveChangesAsync();
        var uri = await _db.CustomerEntities.CountAsync();
        return CreatedAtRoute();
        
        //to do

    }

}