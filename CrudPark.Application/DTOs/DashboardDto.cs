namespace CrudPark.Application.DTOs;

public class DashboardDto
{
    public int VehiclesCurrentlyInside { get; set; }
    public double OccupationPercentage { get; set; }
    public decimal IncomeToday { get; set; }
    public int MembershipsActive { get; set; }
    public int MembershipsExpiringSoon { get; set; }
    public int MembershipsExpired { get; set; }
}