using System.Security.Claims;
using EventsApi.Domain.Entities;
using EventsApi.Models;
using EventsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.resgeneral;

[ApiController]
[Authorize]

[Route("api/[controller]")]
public class InscripcionController : ControllerBase
{
    private readonly InscripcionService _inscripcionService;
    private readonly EventoService _eventoService;

    public InscripcionController(InscripcionService inscripcionService, EventoService eventoService)
    {
        _inscripcionService = inscripcionService;
        _eventoService = eventoService;
    }
    [HttpPost("registrar")]
    public async Task<IActionResult> RegistrarInscripcion([FromBody] Inscripcion inscripcion)
    {
        // Validar el modelo
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                Error = true,
                Mensaje = "Datos de inscripción inválidos.",
                Detalles = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        try
        {
            // Obtener el usuario desde el token
            string? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int usuarioId))
            {
                return Unauthorized(new
                {
                    Error = true,
                    Mensaje = "El token no contiene un ID de usuario válido."
                });
            }

            // Procesar la inscripción
            RespuestaGeneral<object> respuesta = await _inscripcionService.RegistrarInscripcionAsync(usuarioId, inscripcion.EventoId);

            if (respuesta.Error)
            {
                return BadRequest(respuesta);
            }

            return Ok(respuesta);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Error = true,
                Mensaje = "Ocurrió un error interno en el servidor.",
                Detalles = ex.Message
            });
        }
    }
    [HttpPost("eventoxusuario")]
    
    public async Task<IActionResult> RegistrarInscEventoxUsuario([FromBody] Inscripcion inscripcion)
    {
        // Validar el modelo
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                Error = true,
                Mensaje = "Datos de inscripción inválidos.",
                Detalles = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        try
        {
            // Obtener el usuario desde el token
            string? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int usuarioId))
            {
                return Unauthorized(new
                {
                    Error = true,
                    Mensaje = "El token no contiene un ID de usuario válido."
                });
            }

            // Procesar la inscripción
            RespuestaGeneral<object> respuesta = await _inscripcionService.RegistrarInscripcionAsync(usuarioId, inscripcion.EventoId);

            if (respuesta.Error)
            {
                return BadRequest(respuesta);
            }

            return Ok(respuesta);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Error = true,
                Mensaje = "Ocurrió un error interno en el servidor.",
                Detalles = ex.Message
            });
        }
    }


    [HttpGet("usuariosinscritos/{eventoId}")]
public async Task<IActionResult> ObtenerUsuariosInscritos(int eventoId)
{
    // Validar si el evento existe
    Evento? evento = await _eventoService.GetByIdAsync(eventoId);
    if (evento == null)
    {
        return NotFound(new
        {
            Error = true,
            Mensaje = "El evento no existe."
        });
    }

    IEnumerable<UsuarioInscritoDto> usuarios = await _inscripcionService.ObtenerUsuariosInscritosAsync(eventoId);

    if (!usuarios.Any())
    {
        return Ok(new
        {
            Error = false,
            Mensaje = "No hay usuarios inscritos en este evento.",
            Resultado = usuarios
        });
    }

    return Ok(new
    {
        Error = false,
        Mensaje = "Usuarios inscritos obtenidos con éxito.",
        Resultado = usuarios
    });
}
    [HttpGet("inscripciones/usuario/{usuarioId}")]
    public async Task<IActionResult> ObtenerInscripcionesPorUsuario(int usuarioId)
    {
        try
        { RespuestaGeneral<IEnumerable<Evento>> respuestaEventos = await _inscripcionService.ObtenerEventosDisponiblesAsync(usuarioId);
            if (respuestaEventos.Error || respuestaEventos.Resultado == null || !respuestaEventos.Resultado.Any())
            {
                return NotFound(new
                {
                    Error = true,
                    Mensaje = "No se encontraron inscripciones para el usuario."
                });
            }

            return Ok(new
            {
                Error = false,
                Mensaje = "Inscripciones obtenidas con éxito.",
                Resultado = respuestaEventos.Resultado
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Error = true,
                Mensaje = "Ocurrió un error interno en el servidor.",
                Detalles = ex.Message
            });
        }
    }
}
