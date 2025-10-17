// Archivo: CrudPark.Application/Services/OperatorService.cs
using CrudPark.Application.DTOs;
using CrudPark.Application.Interfaces;
using CrudPark.Core.Entities;
using BCrypt.Net; // ¡Importante!

namespace CrudPark.Application.Services
{
    public class OperatorService : IOperatorService
    {
        private readonly IOperatorRepository _operatorRepository;

        public OperatorService(IOperatorRepository operatorRepository)
        {
            _operatorRepository = operatorRepository;
        }


        public async Task<IEnumerable<OperatorDto>> GetAllOperatorsAsync()
        {
            var operators = await _operatorRepository.GetAllAsync();
            
            return operators.Select(op => new OperatorDto
            {
                OperatorId = op.OperatorId,
                FullName = op.FullName,
                Email = op.Email,
                IsActive = op.IsActive
            });
        }

        
        public async Task<OperatorDto> CreateOperatorAsync(CreateOperatorDto createOperatorDto)
        {
           
            var newOperator = new Operator
            {
                FullName = createOperatorDto.FullName,
                Email = createOperatorDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(createOperatorDto.Password),
                IsActive = true, // Por defecto, los nuevos operadores están activos
                CreatedAt = DateTime.UtcNow
            };
            
            var createdEntity = await _operatorRepository.AddAsync(newOperator);
            
            return new OperatorDto
            {
                OperatorId = createdEntity.OperatorId,
                FullName = createdEntity.FullName,
                Email = createdEntity.Email,
                IsActive = createdEntity.IsActive
            };
        }
        
        public async Task<OperatorDto?> GetOperatorByIdAsync(int id)
        {
            var op = await _operatorRepository.GetByIdAsync(id);

            // Si el operador no se encuentra, devolvemos null.
            if (op == null)
            {
                return null;
            }

            // Si se encuentra, lo mapeamos a su DTO y lo devolvemos.
            return new OperatorDto
            {
                OperatorId = op.OperatorId,
                FullName = op.FullName,
                Email = op.Email,
                IsActive = op.IsActive
            };
        }
    }
}