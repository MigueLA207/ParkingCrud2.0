namespace CrudPark.Core.Entities;

public class Payment
{
    public int PaymentId { get; set; }
    public int StayId { get; set; } // FK a la tabla Stays
    public decimal Amount { get; set; }
    public DateTime PaymentTimestamp { get; set; }
    public string PaymentMethod { get; set; }
    public int OperatorId { get; set; }

   
    public virtual Stay Stay { get; set; } 
    public virtual Operator Operator { get; set; } 
}