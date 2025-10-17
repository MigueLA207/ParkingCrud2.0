using CrudPark.Core.Entities; 

namespace CrudPark.Application.Interfaces
{
    public interface IOperatorRepository
    {
        Task<IEnumerable<Operator>> GetAllAsync();
        Task<Operator> AddAsync(Operator entity);
        Task<Operator?> GetByIdAsync(int id); 
        Task UpdateAsync(Operator entity); // <-- AÑADIR
        Task DeleteAsync(Operator entity);
    }
}