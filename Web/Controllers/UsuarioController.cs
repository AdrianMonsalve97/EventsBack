using System.Security.Claims;
using EventsApi.Application.DTO;
using EventsApi.Domain.Entities;
using EventsApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.resgeneral;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioService _usuarioService;

    public UsuarioController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet("listar")]
    public async Task<IActionResult> ListarUsuarios()
    {
        // Validar si el usuario es administrador
        string? roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
        if (string.IsNullOrEmpty(roleClaim) || roleClaim != "Administrador")
        {
            return StatusCode(403, new
            {
                Error = true,
                Mensaje = "No tienes permisos para realizar esta acción."
            });
        }

        RespuestaGeneral<List<UsuarioDto>> respuesta = await _usuarioService.ObtenerTodosLosUsuariosAsync();

        if (respuesta.Error)
        {
            return BadRequest(respuesta);
        }

        return Ok(respuesta);
    }

    // Actualizar un usuario
    [HttpPut("actualizar/{id}")]
    public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] Usuario usuarioActualizado)
    {
        // Obtener el ID y el rol del usuario desde el token
        string? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string? roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized(new
            {
                Error = true,
                Mensaje = "El token no contiene un ID de usuario válido."
            });
        }
        // Validar permisos: solo el usuario o un administrador pueden actualizar
        if (userId != id && roleClaim != "Administrador")
        {
            return StatusCode(403, new
            {
                Error = true,
                Mensaje = "No tienes permisos para actualizar este usuario."
            });
        }

        RespuestaGeneral<object> respuesta = await _usuarioService.ActualizarUsuarioAsync(id, usuarioActualizado);

        if (respuesta.Error)
        {
            return BadRequest(respuesta);
        }

        return Ok(respuesta);
    }


}

