using System.Net;
using AutoMapper;
using Investments.Models.Api;
using Investments.Repositories;
using Microsoft.EntityFrameworkCore;
using RentACar.Data;
using RentACar.Models.Db;
using RentACar.Models.Dto;

namespace Investments.Services;

public class CustomerService : ICustomerService
{
    private readonly IMapper _mapper;
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(IMapper mapper, ICustomerRepository customerRepository)
    {
        _mapper = mapper;
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<CustomerDTO>> GetCustomers()
    {
        var result = await _customerRepository.GetCustomers();
        var mappedResult = _mapper.Map<List<CustomerDTO>>(result);
        return mappedResult;
    }

    public async Task<CustomerDTO> GetCustomer(int id)
    {
        if (id == 0)
        {
            throw new Exception("Id cannot equal 0");
        }

        var result = await _customerRepository.GetCustomer(id);
        if (result == null)
        {
            throw new Exception("Customer with id: " + id + " is not registered");
        }

        var mappedResult = _mapper.Map<CustomerDTO>(result);
        return mappedResult;
    }

    public async Task<CustomerDTO> RegisterCustomer(CustomerDTO customerDTO)
    {
        if (await _customerRepository.CustomerNameValidation(customerDTO))
        {
            throw new Exception("This customer is already registered");
        }

        if (customerDTO == null || customerDTO.Id != 0)
        {
            throw new Exception("Provided registration data is incorrect");
        }
        
        var entity = _mapper.Map<CustomerEntity>(customerDTO);

        await _customerRepository.RegisterCustomer(entity);
        await _customerRepository.SaveChanges();
        var lastId = await _customerRepository.LastCustomerId();
        customerDTO.Id = lastId;
        return customerDTO;
    }

    public async Task<string> DeleteCustomer(int id)
    {
        if (id <= 0)
        {
            throw new Exception("Id cannot be equal or lesser than 0");
        }

        var entity = await _customerRepository.GetCustomer(id);

        if (entity == null)
        {
            throw new Exception("Customer with id: " + id + " is not registered");
        }

        _customerRepository.RemoveCustomer(entity);
        await _customerRepository.SaveChanges();
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
        
        var entity = await _customerRepository.GetCustomer(id);

        if (entity == null)
        {
            throw new Exception("Customer with id: " + id + " is not registered");
        }

        var model = _mapper.Map<CustomerEntity>(customerDTO);
        _customerRepository.UpdateCustomer(model);
        await _customerRepository.SaveChanges();
        return "Customer with id: " + id + " was updated successfully";
    }
}