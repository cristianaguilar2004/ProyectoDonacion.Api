using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoDonacion.Common;
using ProyectoDonacion.DTOs.Categorias;
using ProyectoDonacion.Services.Categorias;

namespace ProyectoDonacion.Controllers;

[Route("categorias")]
[ApiController]
[Authorize]
public class CategoriaController : ControllerBase
{
    private readonly CategoriaService _categoriaService;

    public CategoriaController(CategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategorias([FromQuery] bool soloActivos = true)
    {
        var response = await _categoriaService.GetAllCategorias(soloActivos);

        if (response.Type != ResponseType.Ok)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoriaById([FromRoute] string id)
    {
        var response = await _categoriaService.GetCategoriaById(id);

        if (response.Type != ResponseType.Ok)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategoria([FromBody] CreateCategoriaDto dto)
    {
        var response = await _categoriaService.CreateCategoria(dto);

        if (response.Type != ResponseType.Ok)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCategoria([FromBody] UpdateCategoriaDto dto)
    {
        var response = await _categoriaService.UpdateCategoria(dto);

        if (response.Type != ResponseType.Ok)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> ChangeStateCategoria([FromRoute] string id)
    {
        var response = await _categoriaService.ChangeStateCategoria(id);

        if (response.Type != ResponseType.Ok)
            return BadRequest(response);

        return Ok(response);
    }
}
