using MediatR;
using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.CarritoCompra.Persistencia;

namespace TiendaServicios.Api.CarritoCompra.Application
{
    public class Consulta
    {
        public class Ejecuta : IRequest<CarritoDTO>
        {
            public int CarritoSesionId { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, CarritoDTO>
        {
            private readonly CarritoContexto _contexto;

            public Manejador(CarritoContexto contexto)
            {
                _contexto = contexto;
            }

            public async Task<CarritoDTO> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var carritoSesion = await _contexto.CarritoSesion
                    .FirstOrDefaultAsync(x => x.CarritoSesionId == request.CarritoSesionId);

                var carritoSesionDetalle = _contexto.CarritoSesionDetalle
                    .Where(x => x.CarritoSesionId == request.CarritoSesionId)
                    .ToList();

                List<CarritoDetalleDTO> carritoDetallesDTOList = new List<CarritoDetalleDTO>();
                foreach (var libro in carritoSesionDetalle)
                {
                    var carritoDetalle = new CarritoDetalleDTO
                    {
                        LibroId = Guid.TryParse(libro.ProductoSeleccionado, out var guid) ? guid : null,
                        TituloLibro = null, // No hay datos de libro externo
                        FechaPublicacion = null // No hay datos de libro externo
                    };
                    carritoDetallesDTOList.Add(carritoDetalle);
                }

                var carritoSesionDTO = new CarritoDTO
                {
                    CarritoId = carritoSesion.CarritoSesionId,
                    FechaCreacionSesion = carritoSesion.FechaCreacion,
                    ListaProductos = carritoDetallesDTOList
                };

                return carritoSesionDTO;
            }
        }
    }
}
