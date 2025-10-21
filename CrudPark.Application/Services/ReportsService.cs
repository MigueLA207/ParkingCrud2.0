
using CrudPark.Application.DTOs;
using CrudPark.Application.Interfaces;
using Microsoft.Extensions.Configuration; 

namespace CrudPark.Application.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IReportsRepository _reportsRepository;
        private readonly IConfiguration _configuration; 


        public ReportsService(IReportsRepository reportsRepository, IConfiguration configuration)
        {
            _reportsRepository = reportsRepository;
            _configuration = configuration;
        }


 
        public async Task<IEnumerable<DailyIncomeDto>> GetDailyIncomeReportAsync(DateTime startDate, DateTime endDate)
        {

            if (startDate > endDate)
            {
                throw new ArgumentException("La fecha de inicio no puede ser posterior a la fecha de fin.");
            }
            return await _reportsRepository.GetDailyIncomeAsync(startDate, endDate);
        }
        

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

            if (!stays.Any())
            {
                return new OccupationReportDto { AverageOccupationPercentage = 0, AverageStayDuration = TimeSpan.Zero };
            }


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


            var totalSpaces = _configuration.GetValue<int>("AppSettings:TotalParkingSpaces");
            

            if (totalSpaces <= 0)
            {
                return new OccupationReportDto
                {
                    AverageOccupationPercentage = 0, 
                    AverageStayDuration = averageDuration
                };
            }
            

            var totalHoursInPeriod = (endDate - startDate).TotalHours;
            if (totalHoursInPeriod <= 0) return new OccupationReportDto { AverageOccupationPercentage = 0, AverageStayDuration = averageDuration };

            var totalAvailableSpaceHours = totalSpaces * totalHoursInPeriod;

     
            var totalOccupiedSpaceHours = stays.Sum(s => 

                ((s.ExitTimestamp ?? endDate) - s.EntryTimestamp).TotalHours
            );

            double occupationPercentage = (totalOccupiedSpaceHours / totalAvailableSpaceHours) * 100;
            

            occupationPercentage = Math.Min(occupationPercentage, 100);

            return new OccupationReportDto
            {
                AverageOccupationPercentage = Math.Round(occupationPercentage, 2),
                AverageStayDuration = averageDuration
            };
        }
    }
}