using CrudPark.Application.DTOs;

namespace CrudPark.Application.Interfaces
{
    public interface IReportsService
    {
        Task<IEnumerable<DailyIncomeDto>> GetDailyIncomeReportAsync(DateTime startDate, DateTime endDate);
        Task<MembershipVsGuestDto> GetMembershipVsGuestReportAsync(DateTime startDate, DateTime endDate);
        Task<OccupationReportDto> GetOccupationReportAsync(DateTime startDate, DateTime endDate);
    }
}