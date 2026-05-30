using Microsoft.AspNetCore.Mvc;
using ProyectoDonacion.Common;
using ProyectoDonacion.DTOs.Auth;
using ProyectoDonacion.Services.Auth;

namespace ProyectoDonacion.Controllers;

[Route("roles")]
[ApiController]
public class RolController : ControllerBase
{
    private readonly RolService _rolService;

    public RolController(RolService rolService)
    {
        _rolService = rolService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRoles()
    {
        var response = await _rolService.GetAllRoles();

        if (response.Type != ResponseType.Ok)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRol([FromBody] CreateRolDto dto)
    {
        var response = await _rolService.CreateRol(dto);

        if (response.Type != ResponseType.Ok)
            return BadRequest(response);

        return Ok(response);

    }
}
