using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.Aplicacion;
using TiendaServicios.Api.Autor.Modelo;

namespace TiendaServicios.Api.Autor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AutorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Endpoint para crear un nuevo autor
        [HttpPost]
        public async Task<ActionResult<Unit>> CrearAutor(Nuevo.CrearAutor data)
        {
            return await _mediator.Send(data);
        }

        // Endpoint para obtener la lista de autores
        [HttpGet]
        public async Task<ActionResult<List<AutorDto>>> GetAutores()
        {
            return await _mediator.Send(new Consulta.ListaAutor());
        }

        // Endpoint para crear un grado académico
        [HttpPost("grado")]
        public async Task<ActionResult<Unit>> CrearGrado(NuevoGrado.CrearGradoAcademico data)
        {
            return await _mediator.Send(data);
        }

        // Endpoint para obtener la lista de grados académicos
        [HttpGet("grado")]
        public async Task<ActionResult<List<GradoAcademicoDto>>> GetGrados()
        {
            return await _mediator.Send(new ConsultaGrado.ListaGrado());
        }
    }
}
