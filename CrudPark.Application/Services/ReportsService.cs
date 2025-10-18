using CrudPark.Application.DTOs;
using CrudPark.Application.Interfaces;

namespace CrudPark.Application.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IReportsRepository _reportsRepository;

        public ReportsService(IReportsRepository reportsRepository)
        {
            _reportsRepository = reportsRepository;
        }

        public async Task<IEnumerable<DailyIncomeDto>> GetDailyIncomeReportAsync(DateTime startDate, DateTime endDate)
        {
            // Aquí podríamos añadir lógica de negocio, como validar que el rango no sea mayor a un año.
            // Por ahora, pasamos la llamada directamente al repositorio.
            return await _reportsRepository.GetDailyIncomeAsync(startDate, endDate);
        }
        
        public async Task<MembershipVsGuestDto> GetMembershipVsGuestReportAsync(DateTime startDate, DateTime endDate)
        {
            return await _reportsRepository.GetMembershipVsGuestAsync(startDate, endDate);
        }
        
        public async Task<OccupationReportDto> GetOccupationReportAsync(DateTime startDate, DateTime endDate)
        {
            var stays = await _reportsRepository.GetStaysForOccupationReportAsync(startDate, endDate);

            if (!stays.Any())
            {
                return new OccupationReportDto { AverageOccupationPercentage = 0, AverageStayDuration = TimeSpan.Zero };
            }

            // Calculamos la duración promedio de las estancias
            double totalMinutesStayed = 0;
            int completedStays = 0;

            foreach (var stay in stays)
            {
                if (stay.ExitTimestamp.HasValue)
                {
                    totalMinutesStayed += (stay.ExitTimestamp.Value - stay.EntryTimestamp).TotalMinutes;
                    completedStays++;
                }
            }
        
            var averageMinutes = completedStays > 0 ? totalMinutesStayed / completedStays : 0;
            var averageDuration = TimeSpan.FromMinutes(averageMinutes);

            // Para el % de ocupación, necesitaríamos saber la capacidad total del parqueadero.
            // Como no tenemos ese dato, lo dejaremos en 0 por ahora. Es una métrica que requiere más info.
            double occupationPercentage = 0.0; 

            return new OccupationReportDto
            {
                AverageOccupationPercentage = occupationPercentage,
                AverageStayDuration = averageDuration
            };
        }
    }
}