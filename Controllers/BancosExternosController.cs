using Microsoft.AspNetCore.Mvc;
using BankLink.Models;
using BankLink.Interfaces;
using BankLink.DTOs;

namespace BankLink.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BancosExternosController : ControllerBase
{
    private readonly IBancoExternoService _bancoService;

    public BancosExternosController(IBancoExternoService bancoService)
    {
        _bancoService = bancoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BancoExterno>>> GetAll()
    {
        return Ok(await _bancoService.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BancoExterno>> GetById(int id)
    {
        var banco = await _bancoService.GetById(id);
        if (banco == null) return NotFound(new { mensaje = "Banco externo no encontrado" });
        return Ok(banco);
    }

    [HttpPost]
    public async Task<ActionResult<BancoExterno>> Create([FromBody] BancoExternoCreateDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre) || string.IsNullOrWhiteSpace(dto.UrlApi))
            return BadRequest(new { mensaje = "El nombre y la URL de la API son obligatorios." });

        var creado = await _bancoService.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] BancoExternoUpdateDTO dto)
    {
        var actualizado = await _bancoService.Update(id, dto);
        if (!actualizado) return NotFound(new { mensaje = "Banco no encontrado para actualizar." });
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _bancoService.Delete(id);
        if (!eliminado) return NotFound(new { mensaje = "Banco no encontrado para eliminar." });
        return NoContent();
    }
}