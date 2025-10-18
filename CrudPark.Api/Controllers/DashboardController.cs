using CrudPark.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrudPark.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;
    
    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }
    
    [HttpGet("metrics")]
    public async Task<IActionResult> GetDashboardMetrics()
    {
        var metrics = await _dashboardService.GetDashboardMetricsAsync();
        return Ok(metrics);
    }
}