using CrudPark.Core.Entities;
namespace CrudPark.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> FindByEmailAsync(string email);
        Task<Customer> AddAsync(Customer customer);
    }
}