// --- USINGS NECESARIOS ---
using CrudPark.Application.DTOs;
using CrudPark.Application.Interfaces;
using Microsoft.Extensions.Configuration; 

namespace CrudPark.Application.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IReportsRepository _reportsRepository;
        private readonly IConfiguration _configuration; // Para leer appsettings.json

        // Inyectamos tanto el repositorio de reportes como la configuración
        public ReportsService(IReportsRepository reportsRepository, IConfiguration configuration)
        {
            _reportsRepository = reportsRepository;
            _configuration = configuration;
        }

        /// <summary>
        /// Obtiene un resumen de los ingresos totales agrupados por día para un rango de fechas.
        /// </summary>
        public async Task<IEnumerable<DailyIncomeDto>> GetDailyIncomeReportAsync(DateTime startDate, DateTime endDate)
        {
            // La lógica está en el repositorio, el servicio solo valida y pasa la llamada.
            if (startDate > endDate)
            {
                throw new ArgumentException("La fecha de inicio no puede ser posterior a la fecha de fin.");
            }
            return await _reportsRepository.GetDailyIncomeAsync(startDate, endDate);
        }
        
        /// <summary>
        /// Obtiene una comparativa de ingresos y número de entradas entre invitados y miembros con mensualidad.
        /// </summary>
        public async Task<MembershipVsGuestDto> GetMembershipVsGuestReportAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("La fecha de inicio no puede ser posterior a la fecha de fin.");
            }
            return await _reportsRepository.GetMembershipVsGuestAsync(startDate, endDate);
        }
        
        public async Task<OccupationReportDto> GetOccupationReportAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("La fecha de inicio no puede ser posterior a la fecha de fin.");
            }
            
            var stays = await _reportsRepository.GetStaysForOccupationReportAsync(startDate, endDate);

            // Si no hay estancias en el periodo, devolvemos un reporte vacío.
            if (!stays.Any())
            {
                return new OccupationReportDto { AverageOccupationPercentage = 0, AverageStayDuration = TimeSpan.Zero };
            }

            // --- 1. CÁLCULO DE DURACIÓN PROMEDIO ---
            double totalMinutesStayed = 0;
            int completedStays = 0; // Solo contamos las estancias que ya terminaron

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

            // --- 2. CÁLCULO DE PORCENTAJE DE OCUPACIÓN ---
            
            // Leemos la capacidad total desde appsettings.json
            var totalSpaces = _configuration.GetValue<int>("AppSettings:TotalParkingSpaces");
            
            // Si no está configurado o es 0, no podemos calcular el porcentaje.
            if (totalSpaces <= 0)
            {
                return new OccupationReportDto
                {
                    AverageOccupationPercentage = 0, // Devolvemos 0 para no mostrar un dato incorrecto
                    AverageStayDuration = averageDuration
                };
            }
            
            // Calculamos el total de "horas-espacio" disponibles en el periodo
            var totalHoursInPeriod = (endDate - startDate).TotalHours;
            if (totalHoursInPeriod <= 0) return new OccupationReportDto { AverageOccupationPercentage = 0, AverageStayDuration = averageDuration };

            var totalAvailableSpaceHours = totalSpaces * totalHoursInPeriod;

            // Sumamos las horas que cada espacio estuvo ocupado
            var totalOccupiedSpaceHours = stays.Sum(s => 
                // Si la estancia ya terminó, usamos su duración real.
                // Si no ha terminado, contamos su duración hasta el final del periodo del reporte.
                ((s.ExitTimestamp ?? endDate) - s.EntryTimestamp).TotalHours
            );
            
            // Calculamos el porcentaje
            double occupationPercentage = (totalOccupiedSpaceHours / totalAvailableSpaceHours) * 100;
            
            // Nos aseguramos de que no supere el 100% por si hay datos inconsistentes
            occupationPercentage = Math.Min(occupationPercentage, 100);

            // --- 3. DEVOLVER EL DTO CON LOS RESULTADOS ---
            return new OccupationReportDto
            {
                AverageOccupationPercentage = Math.Round(occupationPercentage, 2),
                AverageStayDuration = averageDuration
            };
        }
    }
}