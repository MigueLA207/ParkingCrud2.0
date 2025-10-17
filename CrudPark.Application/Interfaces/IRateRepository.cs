using CrudPark.Core.Entities;

namespace CrudPark.Application.Interfaces;

public interface IRateRepository
{
    Task<IEnumerable<Rate>> GetAllAsync();
    Task<Rate?> GetByIdAsync(int id);
    Task<Rate> AddAsync(Rate rate);
    Task UpdateAsync(Rate rate);
    Task DeleteAsync(Rate rate);
    Task DeactivateAllOthersAsync(int newActiveRateId, string vehicleType);
}