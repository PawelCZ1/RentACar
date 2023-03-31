using RentACar.Models.Dto;

namespace Investments.Services;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDTO>> GetCustomers();

    Task<CustomerDTO> GetCustomer(int id);

    Task<CustomerDTO> RegisterCustomer(CustomerDTO customerDTO);

    Task<string> DeleteCustomer(int id);

    Task<string> UpdateCustomer(int id, CustomerDTO customerDTO);


}