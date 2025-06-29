﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class ConsultaFiltro
    {
        public class LibroUnico : IRequest<LibroMaterialDto>
        {
            public Guid LibroId { get; set; }
        }

        public class Manejador : IRequestHandler<LibroUnico, LibroMaterialDto>
        {
            private readonly ContextoLibreria _contexto;
            private readonly IMapper _mapper;

            public Manejador(ContextoLibreria contexto, IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }

            public async Task<LibroMaterialDto> Handle(LibroUnico request, CancellationToken cancellationToken)
            {
                // Validar si el ID es válido
                if (request.LibroId == Guid.Empty)
                {
                    throw new ArgumentException("El ID del libro no puede estar vacío.");
                }

                // Buscar el libro en la base de datos
                var libro = await _contexto.LibreriaMaterial
                    .FirstOrDefaultAsync(x => x.LibreriaMaterialId == request.LibroId, cancellationToken);

       
                if (libro == null)
                {
                    throw new Exception("No se encontró el libro");
                }

                // Mapear el resultado al DTO
                return _mapper.Map<LibreriaMaterial, LibroMaterialDto>(libro);
            }
        }
    }
}
