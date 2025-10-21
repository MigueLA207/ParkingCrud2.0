using CrudPark.Application.DTOs;
using CrudPark.Application.Interfaces;
using CrudPark.Core.Entities;

namespace CrudPark.Application.Services
{
    public class RateService : IRateService
    {
        private readonly IRateRepository _rateRepository;

        public RateService(IRateRepository rateRepository)
        {
            _rateRepository = rateRepository;
        }

        public async Task<IEnumerable<RateDto>> GetAllRatesAsync()
        {
            var rates = await _rateRepository.GetAllAsync();

            return rates.Select(MapToDto);
        }

        public async Task<RateDto?> GetRateByIdAsync(int id)
        {
            var rate = await _rateRepository.GetByIdAsync(id);

            return rate == null ? null : MapToDto(rate);
        }

        public async Task<RateDto> CreateRateAsync(CreateOrUpdateRateDto createDto)
        {

            var newRate = new Rate
            {
                Description = createDto.Description,
                VehicleType = createDto.VehicleType,
                HourlyRate = createDto.HourlyRate,
                FractionRate = createDto.FractionRate,
                DailyCap = createDto.DailyCap,
                GracePeriodMinutes = createDto.GracePeriodMinutes,
                IsActive = createDto.IsActive
            };


            var createdEntity = await _rateRepository.AddAsync(newRate);

 
            if (createdEntity.IsActive)
            {
 
                await _rateRepository.DeactivateAllOthersAsync(createdEntity.RateId, createdEntity.VehicleType);
            }


            return MapToDto(createdEntity);
        }

        public async Task<RateDto?> UpdateRateAsync(int id, CreateOrUpdateRateDto updateDto)
        {

            var existingRate = await _rateRepository.GetByIdAsync(id);
            if (existingRate == null)
            {
                return null;
            }

            existingRate.Description = updateDto.Description;
            existingRate.VehicleType = updateDto.VehicleType;
            existingRate.HourlyRate = updateDto.HourlyRate;
            existingRate.FractionRate = updateDto.FractionRate;
            existingRate.DailyCap = updateDto.DailyCap;
            existingRate.GracePeriodMinutes = updateDto.GracePeriodMinutes;
            existingRate.IsActive = updateDto.IsActive;


            await _rateRepository.UpdateAsync(existingRate);


            if (existingRate.IsActive)
            {
  
                await _rateRepository.DeactivateAllOthersAsync(existingRate.RateId, existingRate.VehicleType);
            }

            return MapToDto(existingRate);
        }

        public async Task<bool> DeleteRateAsync(int id)
        {

            var rateToDelete = await _rateRepository.GetByIdAsync(id);
            if (rateToDelete == null)
            {
                return false; 
            }


            await _rateRepository.DeleteAsync(rateToDelete);
            return true;
        }


        #region Métodos Privados de Ayuda (Helpers)
        private RateDto MapToDto(Rate rate)
        {
            return new RateDto
            {
                RateId = rate.RateId,
                Description = rate.Description,
                VehicleType = rate.VehicleType,
                HourlyRate = rate.HourlyRate,
                FractionRate = rate.FractionRate,
                DailyCap = rate.DailyCap,
                GracePeriodMinutes = rate.GracePeriodMinutes,
                IsActive = rate.IsActive
            };
        }

        #endregion
    }
}