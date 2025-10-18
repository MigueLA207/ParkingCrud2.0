using System.Globalization;
using CsvHelper;
using CrudPark.Application.DTOs;
using CrudPark.Application.Interfaces;

public class ExportService : IExportService
{
    public byte[] GenerateDailyIncomeCsv(IEnumerable<DailyIncomeDto> data)
    {
        using (var memoryStream = new MemoryStream())
        using (var writer = new StreamWriter(memoryStream))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteHeader<DailyIncomeDto>();
            csv.NextRecord();
            csv.WriteRecords(data);
            writer.Flush();
            return memoryStream.ToArray();
        }
    }
}