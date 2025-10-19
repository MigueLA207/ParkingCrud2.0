// Archivo: CrudPark.Application/Interfaces/IMembershipRepository.cs
using CrudPark.Core.Entities;

namespace CrudPark.Application.Interfaces
{
    public interface IMembershipRepository
    {
        Task<IEnumerable<Membership>> GetAllAsync(); // AÑADIR
        Task<Membership?> GetByIdAsync(int id); // AÑADIR
        Task<Membership?> FindOverlappingMembershipAsync(string licensePlate, string vehicleType, DateTime startDate, DateTime endDate);
        Task<Membership> AddAsync(Membership membership);
        Task UpdateAsync(Membership membership);
        Task<IEnumerable<Membership>> GetMembershipsExpiringOnDateAsync(DateTime expirationDate);
        Task<int> CountActiveAsync(DateTime date);
        Task<int> CountExpiringSoonAsync(DateTime startDate, DateTime endDate);
        Task<int> CountExpiredAsync(DateTime date);
        
    }
}