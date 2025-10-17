namespace CrudPark.Core.Entities;

public class Membership
{
    public int MembershipId { get; set; }
    public int CustomerId { get; set; } 
    public string LicensePlate { get; set; }
    public string VehicleType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }

    // Propiedad de navegación: Una mensualidad pertenece a UN cliente
    public virtual Customer Customer { get; set; }
}