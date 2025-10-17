
namespace CrudPark.Application.DTOs
{
    public class CreateOperatorDto
    {
        public string Password { get; set; } 
        public string FullName { get; set; }
        public string? Email { get; set; }
    }
}