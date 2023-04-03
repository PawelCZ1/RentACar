using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RentACar.Data;
using RentACar.Models.Db;
using RentACar.Models.Dto;

namespace Investments.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly APIDatabaseContext _db;

    public CustomerRepository(APIDatabaseContext db)
    {
        _db = db;
    }
    
    public async Task<IEnumerable<CustomerEntity>> GetCustomers()
    {
        var result = await _db.CustomerEntities.ToListAsync();
        return result;
    }

    public async Task<CustomerEntity?> GetCustomer(int id)
    {
        var result = await _db.CustomerEntities.FirstOrDefaultAsync(e => e.Id == id);
        return result;
    }

    public async Task<bool> CustomerNameValidation(CustomerDTO customerDTO)
    {
        var result =
            await _db.CustomerEntities.FirstOrDefaultAsync(e =>
                e.Name == customerDTO.Name && e.Surname == customerDTO.Surname) != null;
        return result;
    }

    public async ValueTask<EntityEntry<CustomerEntity>> RegisterCustomer(CustomerEntity entity)
    {
        var result = await _db.CustomerEntities.AddAsync(entity);
        return result;
    }

    public async Task<int> SaveChanges()
    {
        var result = await _db.SaveChangesAsync();
        return result;
    }

    public async Task<int> LastCustomerId()
    {
        var result = await _db.CustomerEntities.MaxAsync(e => e.Id);
        return result;
    }

    public EntityEntry<CustomerEntity> RemoveCustomer(CustomerEntity entity)
    {
        var result = _db.CustomerEntities.Remove(entity);
        return result;
    }

    public EntityEntry<CustomerEntity> UpdateCustomer(CustomerEntity entity)
    {
        var result = _db.CustomerEntities.Update(entity);
        return result;
    }
}