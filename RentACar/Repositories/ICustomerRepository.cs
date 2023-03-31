using Microsoft.EntityFrameworkCore.ChangeTracking;
using RentACar.Models.Db;
using RentACar.Models.Dto;

namespace Investments.Repositories;

public interface ICustomerRepository
{
    Task<IEnumerable<CustomerEntity>> GetCustomers();

    Task<CustomerEntity?> GetCustomer(int id);

    Task<bool> CustomerNameValidation(CustomerDTO customerDTO);

    ValueTask<EntityEntry<CustomerEntity>> RegisterCustomer(CustomerEntity entity);

    Task<int> SaveChanges();

    Task<int> LastCustomerId();

    EntityEntry<CustomerEntity> RemoveCustomer(CustomerEntity entity);
    
    EntityEntry<CustomerEntity> UpdateCustomer(CustomerEntity entity);
    
    
}