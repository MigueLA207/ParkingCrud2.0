
namespace CrudPark.Application.DTOs
{
    public class UpdateOperatorDto
    {
        public string FullName { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        // Opcionalmente, podrías incluir un campo para cambiar la contraseña
        // public string? NewPassword { get; set; }
    }
}