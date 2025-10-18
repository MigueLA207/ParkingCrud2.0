namespace CrudPark.Application.DTOs;

public class OccupationReportDto
{
    public double AverageOccupationPercentage { get; set; } // Ej: 65.5 (%)
    public TimeSpan AverageStayDuration { get; set; } // Ej: "03:30:00" (3 horas y media)
}