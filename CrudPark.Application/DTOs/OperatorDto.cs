namespace CrudPark.Application.DTOs
{
    public class OperatorDto
    {
        public int OperatorId { get; set; }
        public string FullName { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
    }
}