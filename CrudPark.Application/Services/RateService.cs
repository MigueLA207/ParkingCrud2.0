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
            // Usamos el método de ayuda para mapear cada elemento de la lista
            return rates.Select(MapToDto);
        }

        public async Task<RateDto?> GetRateByIdAsync(int id)
        {
            var rate = await _rateRepository.GetByIdAsync(id);
            // Si la tarifa no se encuentra, devolvemos null.
            // Si se encuentra, la mapeamos a su DTO.
            return rate == null ? null : MapToDto(rate);
        }

        public async Task<RateDto> CreateRateAsync(CreateOrUpdateRateDto createDto)
        {
            // 1. Mapeamos del DTO de entrada a la entidad del dominio
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

            // 2. Guardamos la nueva tarifa en la base de datos
            var createdEntity = await _rateRepository.AddAsync(newRate);

            // 3. Lógica de negocio: Si la tarifa que acabamos de crear se marcó
            //    como activa, debemos asegurarnos de que todas las demás estén inactivas.
            if (createdEntity.IsActive)
            {
                // Pasamos el tipo de vehículo a la lógica de desactivación
                await _rateRepository.DeactivateAllOthersAsync(createdEntity.RateId, createdEntity.VehicleType);
            }

            // 4. Mapeamos la entidad creada (ya con su ID) a un DTO para la respuesta
            return MapToDto(createdEntity);
        }

        public async Task<RateDto?> UpdateRateAsync(int id, CreateOrUpdateRateDto updateDto)
        {
            // 1. Buscamos la tarifa que se quiere actualizar
            var existingRate = await _rateRepository.GetByIdAsync(id);
            if (existingRate == null)
            {
                return null; // Si no existe, no podemos actualizarla.
            }

            // 2. Actualizamos las propiedades de la entidad con los valores del DTO
            existingRate.Description = updateDto.Description;
            existingRate.VehicleType = updateDto.VehicleType;
            existingRate.HourlyRate = updateDto.HourlyRate;
            existingRate.FractionRate = updateDto.FractionRate;
            existingRate.DailyCap = updateDto.DailyCap;
            existingRate.GracePeriodMinutes = updateDto.GracePeriodMinutes;
            existingRate.IsActive = updateDto.IsActive;

            // 3. Guardamos los cambios en la base de datos
            await _rateRepository.UpdateAsync(existingRate);

            // 4. Lógica de negocio: Si esta tarifa se marcó como activa,
            //    desactivamos todas las demás.
            if (existingRate.IsActive)
            {
                // Pasamos el tipo de vehículo a la lógica de desactivación
                await _rateRepository.DeactivateAllOthersAsync(existingRate.RateId, existingRate.VehicleType);
            }

            // 5. Mapeamos la entidad actualizada a un DTO para la respuesta
            return MapToDto(existingRate);
        }

        public async Task<bool> DeleteRateAsync(int id)
        {
            // 1. Buscamos la tarifa que se quiere eliminar
            var rateToDelete = await _rateRepository.GetByIdAsync(id);
            if (rateToDelete == null)
            {
                return false; // No se pudo eliminar porque no se encontró.
            }

            // 2. La eliminamos de la base de datos
            await _rateRepository.DeleteAsync(rateToDelete);
            return true;
        }


        #region Métodos Privados de Ayuda (Helpers)

        // Este método privado nos ayuda a no repetir el código de mapeo en cada método público.
        // Es una buena práctica para mantener el código limpio (principio DRY - Don't Repeat Yourself).
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