using CrudPark.Application.DTOs;

namespace CrudPark.Application.Interfaces;

public interface IRateService
{
    Task<IEnumerable<RateDto>> GetAllRatesAsync();
    Task<RateDto?> GetRateByIdAsync(int id);
    Task<RateDto> CreateRateAsync(CreateOrUpdateRateDto createDto);
    Task<RateDto?> UpdateRateAsync(int id, CreateOrUpdateRateDto updateDto);
    Task<bool> DeleteRateAsync(int id);
}