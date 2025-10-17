namespace CrudPark.Core.Entities;

public class Operator
{
    public int OperatorId { get; set; }
    public string PasswordHash { get; set; } 
    public string FullName { get; set; }
    public string? Email { get; set; } 
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}