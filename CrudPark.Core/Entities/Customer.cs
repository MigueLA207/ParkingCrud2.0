namespace CrudPark.Core.Entities;

public class Customer
{
    public int CustomerId { get; set; }
    public string FullName { get; set; }
    public string? Email { get; set; }
        
    // Propiedad de navegación: Un cliente puede tener varias mensualidades
    public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();
}