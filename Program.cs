using lecheriaSC.Core.Interfaces;
using lecheriaSC.Infrastructure.Data;
using lecheriaSC.Infrastructure.Repositories;
using lecheriaSC.Consumos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuración del puerto para Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_PUBLIC_URL")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<SucursalContext>(options =>
    options.UseNpgsql(databaseUrl)
);

builder.Services.AddScoped<IRepositorioSucursal, RepositorioSucursal>();
builder.Services.AddScoped<IRepositorioSolicitud, RepositorioSolicitud>();
builder.Services.AddScoped<IRepositorioInventario, RepositorioInventario>();
builder.Services.AddScoped<IRepositorioReporte, RepositorioReporte>();

// Registrar servicios consumidores
builder.Services.AddHttpClient<ContabilidadService>();
builder.Services.AddHttpClient<RRHHService>();
builder.Services.AddHttpClient<VentasService>();
builder.Services.AddHttpClient<MarketingService>();
builder.Services.AddHttpClient<FabricaService>();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- CORRECCIÓN DEL ERROR DE BASE DE DATOS ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<SucursalContext>();
       
        db.Database.Migrate();
        Console.WriteLine("Migración aplicada correctamente.");
    }
    catch (Exception ex)
    {
        
        Console.WriteLine($"Error durante la migración (Ignorado para permitir inicio): {ex.Message}");
    }
}

app.UseCors("AllowFrontend");

// --- CORRECCIÓN PARA VER SWAGGER EN RAILWAY ---
// Quitamos el 'if (IsDevelopment)' para que Swagger salga siempre.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
