using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TiendaServicios.Api.Libro.Aplicacion;

namespace TiendaServicios.Api.Libro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroMaterialController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LibroMaterialController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/LibroMaterial
        [HttpPost]
        public async Task<ActionResult<Unit>> Crear([FromBody] Nuevo.CrearLibreria data)
        {
            var resultado = await _mediator.Send(data);
            return Ok(resultado);
        }

        // GET: api/LibroMaterial
        [HttpGet]
        public async Task<ActionResult<List<LibroMaterialDto>>> GetLibros()
        {
            var libros = await _mediator.Send(new Consulta.Ejecuta());
            return Ok(libros);
        }

        // GET: api/LibroMaterial/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<LibroMaterialDto>> GetLibroUnico(Guid id)
        {
            try
            {
                var libro = await _mediator.Send(new ConsultaFiltro.LibroUnico { LibroId = id });
                return Ok(libro);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

    }
}
