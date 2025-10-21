
using CrudPark.Application.DTOs;
using CrudPark.Application.Interfaces;
using CrudPark.Core.Entities;

namespace CrudPark.Application.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IMembershipRepository _membershipRepo;
        private readonly ICustomerRepository _customerRepo;

        public MembershipService(IMembershipRepository membershipRepo, ICustomerRepository customerRepo)
        {
            _membershipRepo = membershipRepo;
            _customerRepo = customerRepo;
        }

        public async Task<MembershipDto> CreateMembershipAsync(CreateMembershipDto createDto)
        {
            var newStartDate = createDto.StartDate.ToUniversalTime();
            var newEndDate = createDto.EndDate.ToUniversalTime();

            var overlappingMembership = await _membershipRepo.FindOverlappingMembershipAsync(
                createDto.LicensePlate,
                createDto.VehicleType,
                newStartDate,
                newEndDate);
            if (overlappingMembership != null)
            {
                throw new InvalidOperationException(
                    $"Ya existe una mensualidad para la placa {createDto.LicensePlate} que se solapa con este rango de fechas " +
                    $"(ID de mensualidad existente: {overlappingMembership.MembershipId}).");
            }

            Customer customer = null;
            if (!string.IsNullOrEmpty(createDto.CustomerEmail))
            {
                customer = await _customerRepo.FindByEmailAsync(createDto.CustomerEmail);
            }

            if (customer == null)
            {
                customer = new Customer { FullName = createDto.CustomerName, Email = createDto.CustomerEmail };
                await _customerRepo.AddAsync(customer);
            }

            var newMembership = new Membership
            {
                CustomerId = customer.CustomerId,
                LicensePlate = createDto.LicensePlate,
                VehicleType = createDto.VehicleType,
                StartDate = newStartDate,
                EndDate = newEndDate,
                IsActive = true 
            };

            var createdEntity = await _membershipRepo.AddAsync(newMembership);
            return new MembershipDto
            {
                MembershipId = createdEntity.MembershipId,
                CustomerId = createdEntity.CustomerId,
                LicensePlate = createdEntity.LicensePlate,
                VehicleType = createdEntity.VehicleType,
                StartDate = createdEntity.StartDate,
                EndDate = createdEntity.EndDate,
                IsActive = createdEntity.IsActive
            };
        }

        private MembershipDto MapToDto(Membership membership)
        {
            return new MembershipDto
            {
                MembershipId = membership.MembershipId,
                LicensePlate = membership.LicensePlate,
                VehicleType = membership.VehicleType,
                StartDate = membership.StartDate,
                EndDate = membership.EndDate,
                IsActive = membership.IsActive,
                CustomerId = membership.Customer.CustomerId,
                CustomerName = membership.Customer.FullName,
                CustomerEmail = membership.Customer.Email
            };
        }

        public async Task<IEnumerable<MembershipDto>> GetAllMembershipsAsync()
        {
            var memberships = await _membershipRepo.GetAllAsync();
            return memberships.Select(MapToDto);
        }

        public async Task<MembershipDto?> GetMembershipByIdAsync(int id)
        {
            var membership = await _membershipRepo.GetByIdAsync(id);
            return membership == null ? null : MapToDto(membership);
        }

        public async Task<MembershipDto?> UpdateMembershipAsync(int id, UpdateMembershipDto updateDto)
        {
            var existingMembership = await _membershipRepo.GetByIdAsync(id);
            if (existingMembership == null)
            {
                return null; 
            }


            var overlapping = await _membershipRepo.FindOverlappingMembershipAsync(
                existingMembership.LicensePlate,
                existingMembership.VehicleType,
                updateDto.StartDate.ToUniversalTime(),
                updateDto.EndDate.ToUniversalTime());

 
            if (overlapping != null && overlapping.MembershipId != id)
            {
                throw new InvalidOperationException(
                    $"La actualización entra en conflicto con otra mensualidad existente (ID: {overlapping.MembershipId}).");
            }


            existingMembership.StartDate = updateDto.StartDate.ToUniversalTime();
            existingMembership.EndDate = updateDto.EndDate.ToUniversalTime();
            existingMembership.IsActive = updateDto.IsActive;

            await _membershipRepo.UpdateAsync(existingMembership);

            return MapToDto(existingMembership);
        }

        public async Task<bool> DeleteMembershipAsync(int id)
        {

            var membership = await _membershipRepo.GetByIdAsync(id);
            if (membership == null)
            {
                return false; 
            }


            membership.IsActive = false;


            await _membershipRepo.UpdateAsync(membership);

            return true;
        }
    }
}