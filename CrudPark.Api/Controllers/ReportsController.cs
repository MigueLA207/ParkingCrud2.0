
using CrudPark.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportsService _reportsService;
    private readonly IExportService _exportService;

    public ReportsController(IReportsService reportsService, IExportService exportService)
    {
        _reportsService = reportsService;
        _exportService = exportService;
    }


    [HttpGet("daily-income")]
    public async Task<IActionResult> GetDailyIncomeReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (startDate > endDate)
        {
            return BadRequest("La fecha de inicio no puede ser posterior a la fecha de fin.");
        }

        var reportData = await _reportsService.GetDailyIncomeReportAsync(startDate, endDate);
        return Ok(reportData);
    }
    

    [HttpGet("membership-vs-guest")]
    public async Task<IActionResult> GetMembershipVsGuestReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (startDate > endDate)
        {
            return BadRequest("La fecha de inicio no puede ser posterior a la fecha de fin.");
        }

        var reportData = await _reportsService.GetMembershipVsGuestReportAsync(startDate, endDate);
        return Ok(reportData);
    
    }
    
    [HttpGet("occupation")]
    public async Task<IActionResult> GetOccupationReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (startDate > endDate)
        {
            return BadRequest("La fecha de inicio no puede ser posterior a la fecha de fin.");
        }
        var reportData = await _reportsService.GetOccupationReportAsync(startDate, endDate);
        return Ok(reportData);
    }
    
    [HttpGet("daily-income/export/csv")]
    public async Task<IActionResult> ExportDailyIncomeReportToCsv([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (startDate > endDate)
        {
            return BadRequest("La fecha de inicio no puede ser posterior a la fecha de fin.");
        }


        var reportData = await _reportsService.GetDailyIncomeReportAsync(startDate, endDate);


        var csvBytes = _exportService.GenerateDailyIncomeCsv(reportData);
    

        var fileName = $"IngresosDiarios_{startDate:yyyyMMdd}_a_{endDate:yyyyMMdd}.csv";
    

        return File(csvBytes, "text/csv", fileName);
    }
}