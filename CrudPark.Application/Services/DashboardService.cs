using CrudPark.Application.DTOs;
using CrudPark.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CrudPark.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IStayRepository _stayRepo;
        private readonly IPaymentRepository _paymentRepo;
        private readonly IMembershipRepository _membershipRepo;
        private readonly IConfiguration _configuration;

        public DashboardService(
            IStayRepository stayRepo,
            IPaymentRepository paymentRepo,
            IMembershipRepository membershipRepo,
            IConfiguration configuration)
        {
            _stayRepo = stayRepo;
            _paymentRepo = paymentRepo;
            _membershipRepo = membershipRepo;
            _configuration = configuration;
        }

        public async Task<DashboardDto> GetDashboardMetricsAsync()
        {
            var today = DateTime.Now.Date;
            Console.WriteLine("Fecha que vamos a enviar");
            Console.WriteLine(today);
            var soonDate = today.AddDays(3);
            
            
            int vehiclesInside = await _stayRepo.CountVehiclesInsideAsync();
            decimal incomeToday = await _paymentRepo.GetTotalIncomeForDateAsync(today);
            int activeMemberships = await _membershipRepo.CountActiveAsync(today);
            int expiringSoonMemberships = await _membershipRepo.CountExpiringSoonAsync(today, soonDate);
            int expiredMemberships = await _membershipRepo.CountExpiredAsync(today);
            

            double occupationPercentage = 0.0;
            var totalSpaces = _configuration.GetValue<int>("AppSettings:TotalParkingSpaces");

            if (totalSpaces > 0 && vehiclesInside > 0)
            {
                occupationPercentage = ((double)vehiclesInside / totalSpaces) * 100;
            }


            return new DashboardDto
            {
                VehiclesCurrentlyInside = vehiclesInside,
                OccupationPercentage = Math.Round(occupationPercentage, 2),
                IncomeToday = incomeToday,
                MembershipsActive = activeMemberships,
                MembershipsExpiringSoon = expiringSoonMemberships,
                MembershipsExpired = expiredMemberships
            };
        }
    }
}