using CrudPark.Application.Interfaces; 
using CrudPark.Core.Entities;
using CrudPark.Infrastructure.Data; 
using Microsoft.EntityFrameworkCore;

namespace CrudPark.Infrastructure.Repositories
{
    // Esta clase "cumple el contrato" de IOperatorRepository
    public class OperatorRepository : IOperatorRepository
    {
        private readonly ParkingDbContext _context;

        public OperatorRepository(ParkingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Operator>> GetAllAsync()
        {
            // La lógica para hablar con la BD ahora vive aquí,
            // no en el controlador.
            return await _context.Operators.ToListAsync();
        }
        
        public async Task<Operator> AddAsync(Operator entity)
        {
            await _context.Operators.AddAsync(entity);
            await _context.SaveChangesAsync(); // Guarda los cambios en la BD
            return entity;
        }
        
        public async Task<Operator?> GetByIdAsync(int id)
        {
            // FindAsync es un método de EF Core optimizado para buscar por clave primaria.
            return await _context.Operators.FindAsync(id);
        }
        
        public async Task UpdateAsync(Operator entity)
        {
            _context.Operators.Update(entity);
            await _context.SaveChangesAsync(); // Guarda los cambios en la BD
        }
        
        public async Task DeleteAsync(Operator entity)
        {
            entity.IsActive = false;
            _context.Operators.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}