// --- USINGS NECESARIOS ---
using CrudPark.Application.Configuration;
using CrudPark.Application.Interfaces;
using CrudPark.Application.Services;
using CrudPark.Infrastructure.Data;
using CrudPark.Infrastructure.Repositories;
using CrudPark.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

// --- CONFIGURACIONES GLOBALES ---
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// =================================================================
// 1. CONFIGURACIÓN DE SERVICIOS (INYECCIÓN DE DEPENDENCIAS)
// =================================================================

// --- Conexión a la Base de Datos ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ParkingDbContext>(options =>
    options.UseNpgsql(connectionString)
        .UseSnakeCaseNamingConvention()); // Opcional, si sigues este patrón

// --- Configuración de CORS ---
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            // ¡REVISA QUE EL PUERTO SEA EL CORRECTO PARA TU FRONTEND!
            policy.WithOrigins("http://localhost:5173", "http://localhost:5174")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// --- Configuración de Email ---
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();

// --- Repositorios ---
builder.Services.AddScoped<IOperatorRepository, OperatorRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();
builder.Services.AddScoped<IRateRepository, RateRepository>();
builder.Services.AddScoped<IStayRepository, StayRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IReportsRepository, ReportsRepository>();

// --- Servicios de Lógica de Negocio ---
builder.Services.AddScoped<IOperatorService, OperatorService>();
builder.Services.AddScoped<IMembershipService, MembershipService>();
builder.Services.AddScoped<IRateService, RateService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IReportsService, ReportsService>();
builder.Services.AddScoped<IExportService, ExportService>();

// --- Servicio en Segundo Plano (Hosted Service) ---
builder.Services.AddHostedService<MembershipExpirationNotifier>();

// --- Servicios del Framework ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// =================================================================
// 2. CONSTRUCCIÓN DE LA APLICACIÓN
// =================================================================
var app = builder.Build();


// =================================================================
// 3. CONFIGURACIÓN DEL PIPELINE DE PETICIONES HTTP
// =================================================================

// Habilita Swagger solo en el entorno de desarrollo
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

// Redirige las peticiones HTTP a HTTPS
app.UseHttpsRedirection();

// Aplica la política de CORS
app.UseCors(MyAllowSpecificOrigins);

// Activa el enrutamiento para que las peticiones lleguen a los controladores correctos
app.MapControllers();


// =================================================================
// 4. EJECUCIÓN DE LA APLICACIÓN
// =================================================================
app.Run();