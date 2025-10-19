// Archivo: CrudPark.Infrastructure/Repositories/MembershipRepository.cs
using CrudPark.Application.Interfaces;
using CrudPark.Core.Entities;
using CrudPark.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CrudPark.Infrastructure.Repositories
{
    public class MembershipRepository : IMembershipRepository
    {
        private readonly ParkingDbContext _context;

        public MembershipRepository(ParkingDbContext context)
        {
            _context = context;
        }

        public async Task<Membership?> GetActiveMembershipByVehicleTypeAsync(string vehicleType, string licensePlate)
        {
            // Busca en la tabla 'Memberships' el primer registro que cumpla todas estas condiciones
            return await _context.Memberships
                .FirstOrDefaultAsync(m => 
                    m.LicensePlate == licensePlate && 
                    m.VehicleType == vehicleType && 
                    m.IsActive == true);
        }

        public async Task<Membership> AddAsync(Membership membership)
        {
            // Añade la nueva entidad al contexto de EF Core
            await _context.Memberships.AddAsync(membership);
            // Guarda todos los cambios pendientes en la base de datos
            await _context.SaveChangesAsync();
            return membership;
        }

        public async Task UpdateAsync(Membership membership)
        {
            // Marca la entidad como modificada
            _context.Memberships.Update(membership);
            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();
        }
        
        public async Task<Membership?> FindOverlappingMembershipAsync(string licensePlate, string vehicleType, DateTime startDate, DateTime endDate)
        {
            // Lógica de solapamiento de fechas:
            // Una membresía existente (startA, endA) se solapa con una nueva (startB, endB) si:
            // (startA <= endB) y (endA >= startB)
            return await _context.Memberships
                .FirstOrDefaultAsync(m =>
                    m.LicensePlate == licensePlate &&
                    m.VehicleType == vehicleType &&
                    m.StartDate <= endDate && // El inicio de la vieja es antes del fin de la nueva
                    m.EndDate >= startDate);  // Y el fin de la vieja es después del inicio de la nueva
        }
        
        public async Task<IEnumerable<Membership>> GetAllAsync()
        {
            // Include(m => m.Customer) carga los datos del cliente relacionado en la misma consulta.
            return await _context.Memberships.Include(m => m.Customer).ToListAsync();
        }

        public async Task<Membership?> GetByIdAsync(int id)
        {
            return await _context.Memberships.Include(m => m.Customer).FirstOrDefaultAsync(m => m.MembershipId == id);
        }
        
        public async Task<IEnumerable<Membership>> GetMembershipsExpiringOnDateAsync(DateTime expirationDate)
        {
            return await _context.Memberships
                .Include(m => m.Customer) 
                .Where(m => m.IsActive && m.EndDate.Date == expirationDate.Date)
                .ToListAsync();
        }
        
        public async Task<int> CountActiveAsync(DateTime date)
        {
            return await _context.Memberships.CountAsync(m => m.IsActive && date >= m.StartDate && date <= m.EndDate);
        }

        public async Task<int> CountExpiringSoonAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Memberships.CountAsync(m => m.IsActive && m.EndDate > startDate && m.EndDate <= endDate);
        }
    
        public async Task<int> CountExpiredAsync(DateTime date)
        {
            return await _context.Memberships.CountAsync(m => m.IsActive && m.EndDate < date);
        }

        
    }
}