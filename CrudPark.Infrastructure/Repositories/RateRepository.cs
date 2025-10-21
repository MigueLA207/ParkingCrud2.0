using CrudPark.Application.Interfaces;
using CrudPark.Core.Entities;
using CrudPark.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CrudPark.Infrastructure.Repositories;

public class RateRepository : IRateRepository
{
    private readonly ParkingDbContext _context;
    public RateRepository(ParkingDbContext context) { _context = context; }

    public async Task<IEnumerable<Rate>> GetAllAsync() => await _context.Rates.ToListAsync();
    public async Task<Rate?> GetByIdAsync(int id) => await _context.Rates.FindAsync(id);
    public async Task<Rate> AddAsync(Rate rate)
    {
        await _context.Rates.AddAsync(rate);
        await _context.SaveChangesAsync();
        return rate;
    }
    public async Task UpdateAsync(Rate rate)
    {
        _context.Rates.Update(rate);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(Rate rate)
    {
        _context.Rates.Remove(rate);
        await _context.SaveChangesAsync();
    }

    public async Task DeactivateAllOthersAsync(int newActiveRateId, string vehicleType)
    {

        await _context.Rates
            .Where(r => r.IsActive && 
                        r.VehicleType == vehicleType && 
                        r.RateId != newActiveRateId)
            .ForEachAsync(r => r.IsActive = false); 

        await _context.SaveChangesAsync();
    }
}