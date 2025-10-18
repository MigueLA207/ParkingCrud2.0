using CrudPark.Application.Interfaces;
using CrudPark.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CrudPark.Infrastructure.Repositories;

public class StayRepository : IStayRepository {
    private readonly ParkingDbContext _context;
    public StayRepository(ParkingDbContext context) { _context = context; }
    public async Task<int> CountVehiclesInsideAsync() => 
        await _context.Stays.CountAsync(s => s.ExitTimestamp == null);
}