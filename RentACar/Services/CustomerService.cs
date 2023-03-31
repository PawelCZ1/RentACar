using System.Net;
using AutoMapper;
using Investments.Models.Api;
using Microsoft.EntityFrameworkCore;
using RentACar.Data;
using RentACar.Models.Db;
using RentACar.Models.Dto;

namespace Investments.Services;

public class CustomerService : ICustomerService
{
    private readonly ApiDatabaseContext _db;
    private readonly IMapper _mapper;

    public CustomerService(ApiDatabaseContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerDTO>> GetCustomers()
    {
        var response = await _db.CustomerEntities.ToListAsync();
        var mappedResponse = _mapper.Map<List<CustomerDTO>>(response);
        return mappedResponse;
    }

    public async Task<CustomerDTO> GetCustomer(int id)
    {
        if (id == 0)
        {
            throw new Exception("Id cannot equal 0");
        }

        var response = await _db.CustomerEntities.FirstOrDefaultAsync(e => e.Id == id);
        if (response == null)
        {
            throw new Exception("Customer with id: " + id + " is not registered");
        }

        var mappedResponse = _mapper.Map<CustomerDTO>(response);
        return mappedResponse;
    }

    public async Task<CustomerDTO> RegisterCustomer(CustomerDTO customerDTO)
    {
        if (await _db.CustomerEntities.FirstOrDefaultAsync(e => e.Name == customerDTO.Name && e.Surname == customerDTO.Surname) != null)
        {
            throw new Exception("This customer is already registered");
        }

        if (customerDTO == null || customerDTO.Id != 0)
        {
            throw new Exception("Provided registration data is incorrect");
        }
        
        var entity = _mapper.Map<CustomerEntity>(customerDTO);

        await _db.CustomerEntities.AddAsync(entity);
        await _db.SaveChangesAsync();
        var lastId = await _db.CustomerEntities.MaxAsync(e => e.Id);
        customerDTO.Id = lastId;
        return customerDTO;
    }

    public async Task<string> DeleteCustomer(int id)
    {
        if (id <= 0)
        {
            throw new Exception("Id cannot be equal or lesser than 0");
        }

        var entity = await _db.CustomerEntities.FirstOrDefaultAsync(e => e.Id == id);

        if (entity == null)
        {
            throw new Exception("Customer with id: " + id + " is not registered");
        }

        _db.CustomerEntities.Remove(entity);
        await _db.SaveChangesAsync();
        return "Customer with id: " + id + " was removed from database";
    }

    public async Task<string> UpdateCustomer(int id, CustomerDTO customerDTO)
    {
        if (id != customerDTO.Id)
        {
            throw new Exception("DTO id does not match route id");
        }
        
        if (customerDTO == null)
        {
            throw new Exception("Provided registration data is incorrect");
        }
        
        var entity = await _db.CustomerEntities.FirstOrDefaultAsync(e => e.Id == id);

        if (entity == null)
        {
            throw new Exception("Customer with id: " + id + " is not registered");
        }

        var model = _mapper.Map<CustomerEntity>(customerDTO);
        _db.CustomerEntities.Update(model);
        await _db.SaveChangesAsync();
        return "Customer with id: " + id + " was updated successfully";
    }
}