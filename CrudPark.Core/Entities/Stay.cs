namespace CrudPark.Core.Entities;

public class Stay
{
    public int StayId { get; set; }
    public string LicensePlate { get; set; }
    public DateTime EntryTimestamp { get; set; }
    public DateTime? ExitTimestamp { get; set; } // Nulo si el vehículo está dentro
    public string StayType { get; set; } // "Membership" o "Guest"
    public string Status { get; set; } // "INSIDE" o "OUTSIDE"
    public string Vehicle_Type {get; set;}
        

    public int EntryOperatorId { get; set; }
    public int? ExitOperatorId { get; set; } // Nulo si está dentro

    
    public virtual Operator EntryOperator { get; set; }
    public virtual Operator ExitOperator { get; set; }
}