using System.Globalization;
using CrudPark.Application.DTOs;
using CrudPark.Application.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using System.Text;

public class ExportService : IExportService
{
    public byte[] GenerateDailyIncomeCsv(IEnumerable<DailyIncomeDto> data)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";", 
            Encoding = Encoding.UTF8
        };

        using (var memoryStream = new MemoryStream())
        using (var writer = new StreamWriter(memoryStream, config.Encoding))
        using (var csv = new CsvWriter(writer, config))
        {
            csv.WriteHeader<DailyIncomeDto>();
            csv.NextRecord();
            csv.WriteRecords(data);
            writer.Flush();
            return memoryStream.ToArray();
        }
    }
}