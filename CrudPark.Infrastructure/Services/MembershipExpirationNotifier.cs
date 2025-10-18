using CrudPark.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CrudPark.Infrastructure.Services
{
    public class MembershipExpirationNotifier : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MembershipExpirationNotifier> _logger;

        public MembershipExpirationNotifier(IServiceProvider serviceProvider, ILogger<MembershipExpirationNotifier> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Servicio de Notificación de Vencimientos iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CheckAndNotifyMemberships();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ocurrió un error en el ciclo de notificación de vencimientos.");
                }
                
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        private async Task CheckAndNotifyMemberships()
        {
            _logger.LogInformation("Ejecutando revisión de membresías próximas a vencer...");

            // Creamos un "scope" de servicios. Esto es OBLIGATORIO en un BackgroundService
            // para poder usar servicios "Scoped" como nuestros repositorios.
            using (var scope = _serviceProvider.CreateScope())
            {
                var membershipRepo = scope.ServiceProvider.GetRequiredService<IMembershipRepository>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                var reminderDate = DateTime.UtcNow.AddDays(3).Date; // Buscamos las que vencen en 3 días

                var expiringMemberships = await membershipRepo.GetMembershipsExpiringOnDateAsync(reminderDate);

                if (!expiringMemberships.Any())
                {
                    _logger.LogInformation("No se encontraron membresías que venzan en 3 días.");
                    return;
                }
                
                foreach (var membership in expiringMemberships)
                {
                    if (!string.IsNullOrEmpty(membership.Customer.Email))
                    {
                        var subject = "Recordatorio: Tu mensualidad de CrudPark está por vencer";
                        var body = $"Hola {membership.Customer.FullName},\n\n" +
                                   $"Te recordamos que tu mensualidad para el vehículo con placa {membership.LicensePlate} vence el {membership.EndDate:D}.\n\n" +
                                   "Puedes pasar por nuestras oficinas para renovarla.\n\n" +
                                   "¡Gracias!";
                        
                        await emailService.SendEmailAsync(membership.Customer.Email, subject, body);
                        _logger.LogInformation($"Correo de recordatorio enviado a: {membership.Customer.Email}");
                    }
                }
            }
        }
    }
}