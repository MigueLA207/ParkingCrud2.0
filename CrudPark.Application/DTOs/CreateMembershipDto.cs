// Archivo: CrudPark.Application/DTOs/CreateMembershipDto.cs
using System.ComponentModel.DataAnnotations; // Necesario para validaciones

namespace CrudPark.Application.DTOs
{
    public class CreateMembershipDto
    {
        // Datos del Cliente a crear o asociar
        [Required]
        [StringLength(150)]
        public string CustomerName { get; set; }

        [EmailAddress] // Validación básica de formato de email
        [StringLength(100)]
        public string? CustomerEmail { get; set; }

        // Datos de la Mensualidad
        [Required]
        [StringLength(20)]
        public string LicensePlate { get; set; }

        [Required]
        [StringLength(50)]
        public string VehicleType { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}