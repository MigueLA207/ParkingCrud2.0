
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

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// --- Configuración de CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins(
                "https://crudpark.netlify.app"// Otro posible puerto
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Si tu frontend usa cookies o autenticación
    });
});

// --- Conexión a la Base de Datos ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ParkingDbContext>(options =>
    options.UseNpgsql(connectionString)
           .UseSnakeCaseNamingConvention());

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

// --- Servicios ---
builder.Services.AddScoped<IOperatorService, OperatorService>();
builder.Services.AddScoped<IMembershipService, MembershipService>();
builder.Services.AddScoped<IRateService, RateService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IReportsService, ReportsService>();
builder.Services.AddScoped<IExportService, ExportService>();

// --- Servicio en Segundo Plano ---
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
// 3. CONFIGURACIÓN DEL PIPELINE HTTP
// =================================================================
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// 🔹 CORS antes de los controladores
app.UseCors(MyAllowSpecificOrigins);

app.MapControllers();

// =================================================================
// 4. EJECUCIÓN
// =================================================================
app.Run();
