namespace CrudPark.Application.DTOs
{
    public class DailyIncomeDto
    {
        // Usamos DateOnly porque no nos importa la hora, solo el día.
        public DateOnly Date { get; set; } 
        public decimal TotalAmount { get; set; }
    }
}

