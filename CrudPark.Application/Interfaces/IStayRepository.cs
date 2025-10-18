namespace CrudPark.Application.Interfaces;

public interface IStayRepository {
    Task<int> CountVehiclesInsideAsync();
}