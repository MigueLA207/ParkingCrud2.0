using CrudPark.Application.DTOs;
namespace CrudPark.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardMetricsAsync();
    
}