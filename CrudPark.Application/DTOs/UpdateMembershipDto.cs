// CrudPark.Application/DTOs/UpdateMembershipDto.cs
using System.ComponentModel.DataAnnotations;
namespace CrudPark.Application.DTOs
{
    public class UpdateMembershipDto
    {
        // Solo permitimos actualizar las fechas, que es lo más común (renovaciones)
        // No permitimos cambiar la placa o el tipo de vehículo para mantener la integridad.
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
        
        [Required]
        public bool IsActive { get; set; }
    }
}