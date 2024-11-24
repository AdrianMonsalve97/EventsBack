using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EventsApi.Services;
using EventsApi.Models;
using Models.resgeneral;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace EventsApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly EventoService _eventService;

        public EventController(EventoService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost("filtrar")]
        public async Task<IActionResult> GetFilteredEvents([FromBody] EventoFiltroDto filtro)
        {
            RespuestaGeneral<IEnumerable<Evento>> respuesta = await _eventService.GetFilteredEventsAsync(filtro);

            if (respuesta.Error)
            {
                return BadRequest(respuesta);
            }

            return Ok(respuesta);
        }

        [HttpPost("crearevento")]
        [Authorize]
        public async Task<IActionResult> CreateEvent([FromBody] Evento evento)
        {
            // Validar modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Error = true,
                    Mensaje = "Datos del evento inválidos.",
                    Detalles = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            // Obtener el ID del usuario desde el token
            string? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new
                {
                    Error = true,
                    Mensaje = "El token no contiene un ID de usuario válido."
                });
            }

            // Procesar la creación del evento
            RespuestaGeneral<object> respuesta = await _eventService.CreateEventAsync(evento, userId);

            if (respuesta.Error)
            {
                return BadRequest(respuesta);
            }

            return Ok(respuesta);
        }

        [HttpPut("actualizarevento/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] Evento updatedEvent)
        {
            // Validar el modelo recibido
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Error = true,
                    Mensaje = "Datos del evento inválidos.",
                    Detalles = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            // Obtener el usuario desde el token
            string? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new
                {
                    Error = true,
                    Mensaje = "El token no contiene un ID de usuario válido."
                });
            }

            // Llamar al servicio pasando el userId
            RespuestaGeneral<object> response = await _eventService.UpdateEventAsync(id, updatedEvent, userId);

            if (response.Error)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }


        [HttpDelete("borrarevento/{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            RespuestaGeneral<object> response = await _eventService.DeleteEventAsync(id);
            return Ok(response);
        }
        [HttpGet("eventos-filtrados")]
        public async Task<IActionResult> GetFilteredAvailableEvents([FromQuery] string? nombre, [FromQuery] string? ubicacion)
        {
            EventoFiltroDto filtro = new EventoFiltroDto
            {
                Nombre = nombre,
                Ubicacion = ubicacion,
                FechaHora = DateTime.Now
            };

            RespuestaGeneral<IEnumerable<Evento>> response = await _eventService.GetFilteredEventsAsync(filtro);
            return Ok(response);
        }

        [HttpGet("eventodetalles/{eventoId}")]
        public async Task<IActionResult> ObtenerEventoConDetalles(int eventoId)
        {
            EventoDto? eventoDto = await _eventService.ObtenerEventoConDetallesAsync(eventoId);

            if (eventoDto == null)
            {
                return NotFound(new
                {
                    Error = true,
                    Mensaje = "Evento no encontrado."
                });
            }

            return Ok(new
            {
                Error = false,
                Mensaje = "Evento obtenido con éxito.",
                Resultado = eventoDto
            });
        }
    }
}
