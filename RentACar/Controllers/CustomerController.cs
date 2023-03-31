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

[Route("api/customer")]
[ApiController]
public class CustomerController : ControllerBase
{
    private APIResponse _response;
    private readonly ICustomerService _service;

    public CustomerController(ICustomerService service)
    {
        _service = service;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetCustomers()
    {
        var result = await _service.GetCustomers();
        _response = new APIResponse(HttpStatusCode.OK, true, null, result);
        return Ok(_response);
    }

    [HttpGet("{id:int}", Name = "GetCustomer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> GetCustomer(int id)
    {
        try
        {
            var result = await _service.GetCustomer(id);
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
            if (e.Message.Equals("Customer with id: " + id + " is not registered"))
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
    public async Task<ActionResult<APIResponse>> RegisterCustomer([FromBody] CustomerDTO customerDTO)
    {
        try
        {
            var result = await _service.RegisterCustomer(customerDTO);
            _response = new APIResponse(HttpStatusCode.Created, true, null, result);
            return CreatedAtRoute("GetCustomer", new {id = result.Id}, _response);
        }
        catch (Exception e)
        {
            if (e.Message.Equals("This customer is already registered"))
            {
                _response = new APIResponse(HttpStatusCode.BadRequest, false,
                    new List<string> { e.Message }, null);
                return BadRequest(_response);
            }

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

    [HttpDelete("{id:int}", Name = "DeleteCustomer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> DeleteCustomer(int id)
    {
        try
        {
            var result = await _service.DeleteCustomer(id);
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

            if (e.Message.Equals("Customer with id: " + id + " is not registered"))
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

    [HttpPut("{id:int}", Name = "UpdateCustomer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> UpdateCustomer(int id, [FromBody] CustomerDTO customerDTO)
    {
        /*if (id != customerDTO.Id)
        {
            _response = new APIResponse(HttpStatusCode.BadRequest, false,
                new List<string> { "DTO id does not match route id" }, null);
            return BadRequest(_response);
        }
        
        if (customerDTO == null)
        {
            _response = new APIResponse(HttpStatusCode.BadRequest, false,
                new List<string> { "Provided registration data is incorrect" }, null);
            return BadRequest(_response);
        }
        
        var entity = await _db.CustomerEntities.FirstOrDefaultAsync(e => e.Id == id);

        if (entity == null)
        {
            _response = new APIResponse(HttpStatusCode.NotFound, false,
                new List<string> { "Customer with id: " + id + " is not registered" }, null);
            return NotFound(_response);
        }

        var model = _mapper.Map<CustomerEntity>(customerDTO);
        _db.CustomerEntities.Update(model);
        await _db.SaveChangesAsync();
        _response = new APIResponse(HttpStatusCode.OK, true,
            null, "Customer with id: " + id + " was updated successfully");
        return Ok(_response);*/
        try
        {
            var result = await _service.UpdateCustomer(id, customerDTO);
            _response = new APIResponse(HttpStatusCode.OK, true,
                null, result);
            return Ok(_response);
        }
        catch (Exception e)
        {
            if (e.Message.Equals("DTO id does not match route id"))
            {
                _response = new APIResponse(HttpStatusCode.BadRequest, false,
                    new List<string> { e.Message }, null);
                return BadRequest(_response);
            }

            if (e.Message.Equals("Provided registration data is incorrect"))
            {
                _response = new APIResponse(HttpStatusCode.BadRequest, false,
                    new List<string> { e.Message }, null);
                return BadRequest(_response);
            }

            if (e.Message.Equals("Customer with id: " + id + " is not registered"))
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