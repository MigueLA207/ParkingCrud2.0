// --- Usings necesarios ---
using CrudPark.Application.Interfaces;
using CrudPark.Application.Services;
using CrudPark.Infrastructure.Data;
using CrudPark.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// --- 1. Configuración de Servicios (Inyección de Dependencias) ---

// Lee la cadena de conexión del archivo appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registra el DbContext para que pueda ser inyectado en otras clases
builder.Services.AddDbContext<ParkingDbContext>(options =>
    options.UseNpgsql(connectionString)
        .UseSnakeCaseNamingConvention());

// Registra las interfaces con sus implementaciones concretas
// Cuando se pida un IOperatorRepository, se entregará un OperatorRepository
builder.Services.AddScoped<IOperatorRepository, OperatorRepository>();
// Cuando se pida un IOperatorService, se entregará un OperatorService
builder.Services.AddScoped<IOperatorService, OperatorService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();
builder.Services.AddScoped<IMembershipService, MembershipService>();
builder.Services.AddScoped<IRateRepository, RateRepository>();
builder.Services.AddScoped<IRateService, RateService>();

// Registra los servicios necesarios para los controladores de API
builder.Services.AddControllers();

// Registra los servicios para la generación de la documentación de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// --- 2. Construcción de la Aplicación ---

var app = builder.Build();


// --- 3. Configuración del Pipeline de Peticiones HTTP ---

// Habilita Swagger solo en el entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirige las peticiones HTTP a HTTPS
app.UseHttpsRedirection();

// ¡ESTA ES LA LÍNEA QUE FALTABA!
// Activa el enrutamiento para que las peticiones lleguen a los controladores correctos
app.MapControllers();


// --- 4. Ejecución de la Aplicación ---

app.Run();