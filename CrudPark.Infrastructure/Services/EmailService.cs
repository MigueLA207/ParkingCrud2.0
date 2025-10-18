using System.Net;
using System.Net.Mail;
using CrudPark.Application.Configuration;
using CrudPark.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace CrudPark.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        // Inyectamos la configuración (que se leerá de user-secrets)
        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            // Verificamos que las credenciales no estén vacías
            if (string.IsNullOrEmpty(_smtpSettings.Username) || string.IsNullOrEmpty(_smtpSettings.Password))
            {
                // Si no hay credenciales configuradas, simplemente no hacemos nada para evitar un error.
                // En una app real, aquí se registraría un log.
                return;
            }

            // SmtpClient es la clase de .NET para enviar correos
            using (var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
            {
                // Usamos las credenciales (tu email y la contraseña de aplicación)
                client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                client.EnableSsl = true; // Gmail requiere una conexión segura (SSL/TLS)

                // Creamos el mensaje de correo
                var fromAddress = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName);
                var toAddress = new MailAddress(toEmail);

                var mailMessage = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false // El cuerpo es texto plano
                };

                // Enviamos el correo de forma asíncrona
                await client.SendMailAsync(mailMessage);
            }
        }
        
        
    }
}