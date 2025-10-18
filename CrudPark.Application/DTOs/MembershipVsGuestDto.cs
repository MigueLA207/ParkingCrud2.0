namespace CrudPark.Application.DTOs
{
    public class MembershipVsGuestDto
    {
        public decimal GuestIncome { get; set; } // Ingresos totales generados por invitados
        public int GuestEntries { get; set; } // Número de entradas de invitados
        public int MembershipEntries { get; set; } // Número de entradas de miembros
    }
}