using AutoMapper;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Aplicacion;

namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Configuración del mapeo entre LibreriaMaterial y LibroMaterialDto
            CreateMap<LibreriaMaterial, LibroMaterialDto>();
        }
    }
}
