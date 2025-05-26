using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.Modelo;
using TiendaServicios.Api.Autor.Persistencia;

namespace TiendaServicios.Api.Autor.Aplicacion
{
    public class NuevoGrado
    {
        public class CrearGradoAcademico : IRequest
        {
            public string Nombre { get; set; }
            public string CentroAcademico { get; set; }
            public DateTime? FechaGrado { get; set; }
            public int AutorLibroId { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<CrearGradoAcademico>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.CentroAcademico).NotEmpty();
                RuleFor(x => x.AutorLibroId).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<CrearGradoAcademico>
        {
            private readonly ContextoAutor _contexto;

            public Manejador(ContextoAutor contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(CrearGradoAcademico request, CancellationToken cancellationToken)
            {
                var gradoAcademico = new GradoAcademico
                {
                    Nombre = request.Nombre,
                    CentroAcademico = request.CentroAcademico,
                    FechaGrado = request.FechaGrado,
                    AutorLibroId = request.AutorLibroId,
                    GradoAcademicoGuid = Guid.NewGuid().ToString()
                };

                _contexto.GradoAcademico.Add(gradoAcademico);
                var valor = await _contexto.SaveChangesAsync();

                if (valor > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el grado académico");
            }
        }
    }

}
