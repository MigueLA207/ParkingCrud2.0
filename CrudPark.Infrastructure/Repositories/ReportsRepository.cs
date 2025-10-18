using CrudPark.Application.DTOs;
using CrudPark.Application.Interfaces;
using CrudPark.Core.Entities;
using CrudPark.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CrudPark.Infrastructure.Repositories
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly ParkingDbContext _context;

        public ReportsRepository(ParkingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DailyIncomeDto>> GetDailyIncomeAsync(DateTime startDate, DateTime endDate)
        {
            // Esta es la consulta LINQ que se traducirá a SQL
            return await _context.Payments
                // 1. Filtramos los pagos que están dentro del rango de fechas
                .Where(p => p.PaymentTimestamp.Date >= startDate.Date && p.PaymentTimestamp.Date <= endDate.Date)
                // 2. Agrupamos todos los pagos por el día en que se hicieron
                .GroupBy(p => p.PaymentTimestamp.Date)
                // 3. Proyectamos los resultados en nuestro DTO
                .Select(group => new DailyIncomeDto
                {
                    Date = DateOnly.FromDateTime(group.Key), // La clave del grupo es la fecha
                    TotalAmount = group.Sum(p => p.Amount) // Sumamos el monto de todos los pagos de ese día
                })
                // 4. Ordenamos por fecha para que la gráfica se vea bien
                .OrderBy(dto => dto.Date)
                .ToListAsync();
        }
        
        public async Task<MembershipVsGuestDto> GetMembershipVsGuestAsync(DateTime startDate, DateTime endDate)
        {
            
            var guestIncome = await _context.Payments
                .Where(p => p.PaymentTimestamp.Date >= startDate.Date && p.PaymentTimestamp.Date <= endDate.Date)
                .SumAsync(p => (decimal?)p.Amount) ?? 0;
            
            var guestEntries = await _context.Stays
                .CountAsync(s => s.StayType == "Guest" && 
                                 s.EntryTimestamp.Date >= startDate.Date && 
                                 s.EntryTimestamp.Date <= endDate.Date);
            
            var membershipEntries = await _context.Stays
                .CountAsync(s => s.StayType == "Membership" && 
                                 s.EntryTimestamp.Date >= startDate.Date && 
                                 s.EntryTimestamp.Date <= endDate.Date);
            
            return new MembershipVsGuestDto
            {
                GuestIncome = guestIncome,
                GuestEntries = guestEntries,
                MembershipEntries = membershipEntries
            };
        }
        
        public async Task<List<Stay>> GetStaysForOccupationReportAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Stays
                .Where(s => s.EntryTimestamp >= startDate && s.EntryTimestamp <= endDate)
                .ToListAsync();
        }
    }
}