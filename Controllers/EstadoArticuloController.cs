using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoDonacion.Common;
using ProyectoDonacion.DTOs.EstadoArticulos;
using ProyectoDonacion.Services.EstadoArticulos;

namespace ProyectoDonacion.Controllers;

[Route("estados-articulo")]
[ApiController]
[Authorize]
public class EstadoArticuloController : ControllerBase
{
    private readonly EstadoArticuloService _estadoArticuloService;

    public EstadoArticuloController(EstadoArticuloService estadoArticuloService)
    {
        _estadoArticuloService = estadoArticuloService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEstadosArticulo([FromQuery] bool soloActivos = true)
    {
        var response = await _estadoArticuloService.GetAllEstadosArticulo(soloActivos);

        if (response.Type != ResponseType.Ok)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEstadoArticuloById([FromRoute] string id)
    {
        var response = await _estadoArticuloService.GetEstadoArticuloById(id);

        if (response.Type != ResponseType.Ok)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEstadoArticulo([FromBody] CreateEstadoArticuloDto dto)
    {
        var response = await _estadoArticuloService.CreateEstadoArticulo(dto);

        if (response.Type != ResponseType.Ok)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateEstadoArticulo([FromBody] UpdateEstadoArticuloDto dto)
    {
        var response = await _estadoArticuloService.UpdateEstadoArticulo(dto);

        if (response.Type != ResponseType.Ok)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> ChangeStateEstadoArticulo([FromRoute] string id)
    {
        var response = await _estadoArticuloService.ChangeStateEstadoArticulo(id);

        if (response.Type != ResponseType.Ok)
            return BadRequest(response);

        return Ok(response);
    }
}
