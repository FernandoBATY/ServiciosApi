using AutoMapper;
using TiendaServicios.Api.Autor.Modelo;
using TiendaServicios.Api.Autor.Aplicacion;

namespace TiendaServicios.Api.Autor.Aplicacion
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Configuración del mapeo entre AutorLibro y AutorDto
            CreateMap<AutorLibro, AutorDto>();
            CreateMap<GradoAcademico, GradoAcademicoDto>();



        }
    }
}