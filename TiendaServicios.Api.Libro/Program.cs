using Microsoft.EntityFrameworkCore;
using MediatR;
using TiendaServicios.Api.Libro.Persistencia;
using TiendaServicios.Api.Libro.Aplicacion; // Para acceder a Nuevo.Manejador

var builder = WebApplication.CreateBuilder(args);

// Configuración de la base de datos
builder.Services.AddDbContext<ContextoLibreria>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerConnection")));

// Registro de MediatR
builder.Services.AddMediatR(typeof(Nuevo.Manejador).Assembly);

// Registro de AutoMapper
builder.Services.AddAutoMapper(typeof(Nuevo.Manejador).Assembly);

// Agregar servicios al contenedor
builder.Services.AddControllers();

// Configuración de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuración del pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
