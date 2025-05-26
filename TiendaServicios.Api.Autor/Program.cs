using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.Autor.Persistencia;
using MediatR;
using TiendaServicios.Api.Autor.Aplicacion;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// Habilitar comportamiento heredado de Npgsql (para evitar la incompatibilidad con fechas sin zona horaria)
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// 🔌 Inyección del DbContext con PostgreSQL
builder.Services.AddDbContext<ContextoAutor>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConexionDatabase")));

// 🔧 Configuración de AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// 🔧 Configuración de FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Nuevo>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// 🔧 Configuración de controladores
builder.Services.AddControllers();

// 🔧 Configuración de MediatR
builder.Services.AddMediatR(typeof(Nuevo.Manejador).Assembly);

// 🔧 Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    });
}

// 🌐 Middleware
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
