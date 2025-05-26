using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.Modelo;
using TiendaServicios.Api.Autor.Persistencia;

namespace TiendaServicios.Api.Autor.Aplicacion
{
    public class ConsultaGrado
    {
        public class ListaGrado : IRequest<List<GradoAcademicoDto>> { }

        public class Manejador : IRequestHandler<ListaGrado, List<GradoAcademicoDto>>
        {
            private readonly ContextoAutor _contexto;
            private readonly IMapper _mapper;

            public Manejador(ContextoAutor contexto, IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }

            public async Task<List<GradoAcademicoDto>> Handle(ListaGrado request, CancellationToken cancellationToken)
            {
                var grados = await _contexto.GradoAcademico.ToListAsync();
                return _mapper.Map<List<GradoAcademico>, List<GradoAcademicoDto>>(grados);
            }
        }
    }
}
