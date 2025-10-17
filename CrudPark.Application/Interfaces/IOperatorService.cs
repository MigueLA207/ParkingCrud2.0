using CrudPark.Application.DTOs;
using CrudPark.Core.Entities;

namespace CrudPark.Application.Interfaces
{
    public interface IOperatorService
    {
        Task<IEnumerable<OperatorDto>> GetAllOperatorsAsync();
        Task<OperatorDto> CreateOperatorAsync(CreateOperatorDto createOperatorDto);
        Task<OperatorDto?> GetOperatorByIdAsync(int id);
    }
}