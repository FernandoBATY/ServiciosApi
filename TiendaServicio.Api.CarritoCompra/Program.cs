using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.CarritoCompra.Persistencia;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Configuración de MySQL para EF Core
builder.Services.AddDbContext<CarritoContexto>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySqlConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySqlConnection"))
    )
);

// Registro de MediatR
builder.Services.AddMediatR(typeof(TiendaServicios.Api.CarritoCompra.Aplicacion.Nuevo).Assembly);

// Servicios básicos de la API
builder.Services.AddControllers();

// Agrega Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Habilita Swagger solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
