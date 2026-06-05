using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoDonacion.Common;
using ProyectoDonacion.DTOs.EstadoDonaciones;
using ProyectoDonacion.Services.EstadoDonaciones;

namespace ProyectoDonacion.Controllers;

[Route("estados-donacion")]
[ApiController]
[Authorize]
public class EstadoDonacionController : ControllerBase
{
    private readonly EstadoDonacionService _estadoDonacionService;

    public EstadoDonacionController(EstadoDonacionService estadoDonacionService)
    {
        _estadoDonacionService = estadoDonacionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEstadosDonacion([FromQuery] bool soloActivos = true)
    {
        var response = await _estadoDonacionService.GetAllEstadosDonacion(soloActivos);

        if (response.Type != ResponseType.Ok)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEstadoDonacionById([FromRoute] string id)
    {
        var response = await _estadoDonacionService.GetEstadoDonacionById(id);

        if (response.Type != ResponseType.Ok)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEstadoDonacion([FromBody] CreateEstadoDonacionDto dto)
    {
        var response = await _estadoDonacionService.CreateEstadoDonacion(dto);

        if (response.Type != ResponseType.Ok)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateEstadoDonacion([FromBody] UpdateEstadoDonacionDto dto)
    {
        var response = await _estadoDonacionService.UpdateEstadoDonacion(dto);

        if (response.Type != ResponseType.Ok)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> ChangeStateEstadoDonacion([FromRoute] string id)
    {
        var response = await _estadoDonacionService.ChangeStateEstadoDonacion(id);

        if (response.Type != ResponseType.Ok)
            return BadRequest(response);

        return Ok(response);
    }
}
