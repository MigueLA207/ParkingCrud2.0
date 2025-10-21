
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
          
            return await _context.Memberships
                .FirstOrDefaultAsync(m => 
                    m.LicensePlate == licensePlate && 
                    m.VehicleType == vehicleType && 
                    m.IsActive == true);
        }

        public async Task<Membership> AddAsync(Membership membership)
        {
 
            await _context.Memberships.AddAsync(membership);
  
            await _context.SaveChangesAsync();
            return membership;
        }

        public async Task UpdateAsync(Membership membership)
        {
 
            _context.Memberships.Update(membership);

            await _context.SaveChangesAsync();
        }
        
        public async Task<Membership?> FindOverlappingMembershipAsync(string licensePlate, string vehicleType, DateTime startDate, DateTime endDate)
        {

            return await _context.Memberships
                .FirstOrDefaultAsync(m =>
                    m.LicensePlate == licensePlate &&
                    m.VehicleType == vehicleType &&
                    m.StartDate <= endDate && 
                    m.EndDate >= startDate);  
        }
        
        public async Task<IEnumerable<Membership>> GetAllAsync()
        {

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