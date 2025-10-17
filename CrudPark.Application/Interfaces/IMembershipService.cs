// Archivo: CrudPark.Application/Interfaces/IMembershipService.cs
using CrudPark.Application.DTOs;

namespace CrudPark.Application.Interfaces
{
    public interface IMembershipService
    {
        // Orquesta la lógica para crear una nueva mensualidad
        Task<IEnumerable<MembershipDto>> GetAllMembershipsAsync(); // AÑADIR
        Task<MembershipDto?> GetMembershipByIdAsync(int id); // AÑADIR
        Task<MembershipDto> CreateMembershipAsync(CreateMembershipDto createDto);
        Task<MembershipDto?> UpdateMembershipAsync(int id, UpdateMembershipDto updateDto); // AÑADIR
        Task<bool> DeleteMembershipAsync(int id);
    }
}