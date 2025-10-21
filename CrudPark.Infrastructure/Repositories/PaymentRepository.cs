using CrudPark.Application.Interfaces;
using CrudPark.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CrudPark.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository {
    private readonly ParkingDbContext _context;
    public PaymentRepository(ParkingDbContext context) { _context = context; }

    // Archivo: CrudPark.Infrastructure/Repositories/PaymentRepository.cs

    public async Task<decimal> GetTotalIncomeForDateAsync(DateTime date)
    {
        // NO necesitamos quemar la fecha ni crear rangos.
        // La consulta LINQ se encarga de todo.
        Console.WriteLine(date.Date);

        var data = await _context.Payments
            // ESTA ES LA LÍNEA CLAVE:
            // Comparamos la parte 'Fecha' de la columna de la BD
            // con la parte 'Fecha' del parámetro que recibimos.
            .Where(p => p.PaymentTimestamp.Date == date.Date)
            .SumAsync(p => (decimal?)p.Amount) ?? 0;

        return data;
    }
}