using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TiendaServicios.Api.CarritoCompra.Persistencia;
using MediatR;


var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de MySQL para EF Core
builder.Services.AddDbContext<CarritoContexto>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySqlConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySqlConnection"))
    )
);

// Registro de MediatR
builder.Services.AddMediatR(typeof(TiendaServicios.Api.CarritoCompra.Aplicacion.Nuevo).Assembly);

// Servicios b�sicos de la API
builder.Services.AddControllers();

// Agrega Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Agrega CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Habilita Swagger solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Habilita CORS antes de Authorization y Endpoints
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
