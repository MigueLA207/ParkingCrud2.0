using CrudPark.Application.DTOs;
public interface IExportService
{
    byte[] GenerateDailyIncomeCsv(IEnumerable<DailyIncomeDto> data);
}