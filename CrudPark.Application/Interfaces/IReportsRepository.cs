using CrudPark.Application.DTOs;
using CrudPark.Core.Entities;

namespace CrudPark.Application.Interfaces
{
    public interface IReportsRepository
    {
        // Este método irá a la BD y devolverá los datos ya agrupados por día.
        Task<IEnumerable<DailyIncomeDto>> GetDailyIncomeAsync(DateTime startDate, DateTime endDate);
        Task<MembershipVsGuestDto> GetMembershipVsGuestAsync(DateTime startDate, DateTime endDate);
        Task<List<Stay>> GetStaysForOccupationReportAsync(DateTime startDate, DateTime endDate);
        
    }
}