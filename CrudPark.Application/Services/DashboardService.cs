using CrudPark.Application.DTOs;
using CrudPark.Application.Interfaces;


namespace CrudPark.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IMembershipRepository _membershipRepo;
    private readonly IStayRepository _stayRepo;
    private readonly IPaymentRepository _paymentRepo;
    
    public DashboardService(IMembershipRepository membershipRepo, IStayRepository stayRepo, IPaymentRepository paymentRepo)
    {
        _membershipRepo = membershipRepo;
        _stayRepo = stayRepo;
        _paymentRepo = paymentRepo;
    }
    
    public async Task<DashboardDto> GetDashboardMetricsAsync()
    {
        var today = DateTime.UtcNow.Date;
        var soonDate = today.AddDays(3);

        // Cada cálculo se delega al repositorio correspondiente
        int vehiclesInside = await _stayRepo.CountVehiclesInsideAsync();
        decimal incomeToday = await _paymentRepo.GetTotalIncomeForDateAsync(today);
        int activeMemberships = await _membershipRepo.CountActiveAsync(today);
        int expiringSoonMemberships = await _membershipRepo.CountExpiringSoonAsync(today, soonDate);
        int expiredMemberships = await _membershipRepo.CountExpiredAsync(today);

        // Construimos el DTO igual que antes
        var dashboardData = new DashboardDto
        {
            VehiclesCurrentlyInside = vehiclesInside,
            IncomeToday = incomeToday,
            MembershipsActive = activeMemberships,
            MembershipsExpiringSoon = expiringSoonMemberships,
            MembershipsExpired = expiredMemberships
        };

        return dashboardData;
    }
    
}