namespace CrudPark.Application.DTOs;

public class CreateOrUpdateRateDto
{
    public string Description { get; set; }
    public string VehicleType { get; set; }
    public decimal HourlyRate { get; set; }
    public decimal FractionRate { get; set; }
    public decimal? DailyCap { get; set; }
    public int GracePeriodMinutes { get; set; }
    public bool IsActive { get; set; }
}