// Archivo: CrudPark.Application/DTOs/MembershipDto.cs
namespace CrudPark.Application.DTOs
{
    public class MembershipDto
    {
        public int MembershipId { get; set; }
        public string LicensePlate { get; set; }
        public string VehicleType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        // Datos del cliente asociado
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
    }
}