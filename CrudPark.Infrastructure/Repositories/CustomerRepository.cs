// Archivo: CrudPark.Infrastructure/Repositories/CustomerRepository.cs
using CrudPark.Application.Interfaces;
using CrudPark.Core.Entities;
using CrudPark.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace CrudPark.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ParkingDbContext _context;
        public CustomerRepository(ParkingDbContext context) { _context = context; }

        public async Task<Customer?> FindByEmailAsync(string email)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }
    }
}