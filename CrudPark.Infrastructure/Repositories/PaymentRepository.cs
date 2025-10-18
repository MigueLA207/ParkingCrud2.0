using CrudPark.Application.Interfaces;
using CrudPark.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CrudPark.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository {
    private readonly ParkingDbContext _context;
    public PaymentRepository(ParkingDbContext context) { _context = context; }
    public async Task<decimal> GetTotalIncomeForDateAsync(DateTime date) =>
        await _context.Payments.Where(p => p.PaymentTimestamp.Date == date).SumAsync(p => (decimal?)p.Amount) ?? 0;
}