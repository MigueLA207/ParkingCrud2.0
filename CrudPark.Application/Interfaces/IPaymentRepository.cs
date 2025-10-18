namespace CrudPark.Application.Interfaces;

public interface IPaymentRepository {
    Task<decimal> GetTotalIncomeForDateAsync(DateTime date);
}