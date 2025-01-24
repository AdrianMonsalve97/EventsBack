using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EventsApi.Services;
using EventsApi.Models;
using Models.resgeneral;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using EventsApi.Domain.Entities;
using EventsApi.Models.DTO;

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
            IEnumerable<EventoDto> respuesta = await _eventService.GetFilteredEventsAsync(filtro);
            return Ok(respuesta);
        }

        [HttpPost("crearevento")]
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
            return Ok(respuesta);
        }

        [HttpPut("actualizarevento/{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] Evento updatedEvent)
        {
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
            return Ok(response);
        }


        [HttpDelete("borrarevento/{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            RespuestaGeneral<object> response = await _eventService.DeleteEventAsync(id);
            return Ok(response);
        }

        [HttpGet("eventos-filtrados")]
        public async Task<IActionResult> GetFilteredAvailableEvents([FromQuery] EventoFiltroDto filtro)
        {
            IEnumerable<EventoDto> response = await _eventService.GetFilteredEventsAsync(filtro);
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

            return Ok(eventoDto);
        }
    }
}