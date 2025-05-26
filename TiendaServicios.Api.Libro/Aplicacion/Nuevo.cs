using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class Nuevo
    {
        // Comando para crear una nueva librería
        public class CrearLibreria : IRequest
        {
            public Guid? LibreriaMaterialId { get; set; }
            public string Titulo { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public Guid? AutorLibro { get; set; }
        }

        // Validación del comando
        public class EjecutaValidacion : AbstractValidator<CrearLibreria>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.LibreriaMaterialId)
                    .NotEmpty().WithMessage("El ID de la librería es obligatorio.");

                RuleFor(x => x.Titulo)
                    .NotEmpty().WithMessage("El título es obligatorio.");

                RuleFor(x => x.FechaPublicacion)
                    .NotEmpty().WithMessage("La fecha de publicación es obligatoria.")
                    .Must(fecha => fecha == null || fecha <= DateTime.Now)
                    .WithMessage("La fecha de publicación no puede ser en el futuro.");

                RuleFor(x => x.AutorLibro)
                    .NotEmpty().WithMessage("El ID del autor es obligatorio.");
            }
        }

        // Manejador para procesar el comando
        public class Manejador : IRequestHandler<CrearLibreria>
        {
            private readonly ContextoLibreria _contexto;

            public Manejador(ContextoLibreria contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(CrearLibreria request, CancellationToken cancellationToken)
            {
                // Validar si ya existe un registro con el mismo ID
                var existeLibreria = await _contexto.LibreriaMaterial
                    .AnyAsync(x => x.LibreriaMaterialId == request.LibreriaMaterialId, cancellationToken);

                if (existeLibreria)
                {
                    throw new Exception("Ya existe una librería con el mismo ID.");
                }

                // Crear el nuevo registro
                var libreriaMaterial = new LibreriaMaterial
                {
                    LibreriaMaterialId = request.LibreriaMaterialId,
                    Titulo = request.Titulo,
                    FechaPublicacion = request.FechaPublicacion,
                    AutorLibro = request.AutorLibro
                };

                _contexto.LibreriaMaterial.Add(libreriaMaterial);
                var resultado = await _contexto.SaveChangesAsync(cancellationToken);

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo guardar la librería.");
            }
        }
    }
}
