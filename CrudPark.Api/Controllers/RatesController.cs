using CrudPark.Application.DTOs;
using CrudPark.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrudPark.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RatesController : ControllerBase
{
    private readonly IRateService _rateService;
    public RatesController(IRateService rateService) { _rateService = rateService; }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _rateService.GetAllRatesAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var rate = await _rateService.GetRateByIdAsync(id);
        return rate == null ? NotFound() : Ok(rate);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrUpdateRateDto dto)
    {
        var newRate = await _rateService.CreateRateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = newRate.RateId }, newRate);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateOrUpdateRateDto dto)
    {
        var updatedRate = await _rateService.UpdateRateAsync(id, dto);
        return updatedRate == null ? NotFound() : Ok(updatedRate);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _rateService.DeleteRateAsync(id);
        return success ? NoContent() : NotFound();
    }
}