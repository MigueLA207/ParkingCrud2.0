
using CrudPark.Application.DTOs;
using CrudPark.Application.Interfaces;
using CrudPark.Core.Entities;
using BCrypt.Net; 

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

      
            if (op == null)
            {
                return null;
            }

     
            return new OperatorDto
            {
                OperatorId = op.OperatorId,
                FullName = op.FullName,
                Email = op.Email,
                IsActive = op.IsActive
            };
        }
        
        public async Task<OperatorDto?> UpdateOperatorAsync(int id, UpdateOperatorDto updateOperatorDto)
        {
            var existingOperator = await _operatorRepository.GetByIdAsync(id);
            if (existingOperator == null)
            {
                return null; 
            }


            existingOperator.FullName = updateOperatorDto.FullName;
            existingOperator.Email = updateOperatorDto.Email;
            existingOperator.IsActive = updateOperatorDto.IsActive;

            await _operatorRepository.UpdateAsync(existingOperator);

     
            return new OperatorDto
            {
                OperatorId = existingOperator.OperatorId,
                FullName = existingOperator.FullName,
                Email = existingOperator.Email,
                IsActive = existingOperator.IsActive
            };
        }

        public async Task<bool> DeleteOperatorAsync(int id)
        {
            var existingOperator = await _operatorRepository.GetByIdAsync(id);
            if (existingOperator == null)
            {
                return false; 
            }

            await _operatorRepository.DeleteAsync(existingOperator);
            return true;
        }
    }
}