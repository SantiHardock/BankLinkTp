using Microsoft.AspNetCore.Mvc;
using BankLink.Models;
using BankLink.Interfaces;
using BankLink.DTOs;

namespace BankLink.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CuentasController : ControllerBase
{
    private readonly ICuentaService _cuentaService;

    public CuentasController(ICuentaService cuentaService)
    {
        _cuentaService = cuentaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cuenta>>> GetAll()
    {
        return Ok(await _cuentaService.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cuenta>> GetById(int id)
    {
        var cuenta = await _cuentaService.GetById(id);
        if (cuenta == null) return NotFound(new { mensaje = "Cuenta no encontrada" });
        return Ok(cuenta);
    }

    [HttpPost]
    public async Task<ActionResult<Cuenta>> Create([FromBody] CuentaCreateDTO dto)
    {
        var creada = await _cuentaService.Create(dto);
        if (creada == null) return BadRequest(new { mensaje = "El ClienteId no existe." });
        
        return CreatedAtAction(nameof(GetById), new { id = creada.Id }, creada);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CuentaUpdateDTO dto)
    {
        var actualizado = await _cuentaService.Update(id, dto);
        if (!actualizado) return NotFound(new { mensaje = "Cuenta no encontrada." });
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _cuentaService.Delete(id);
        if (!eliminado) return NotFound(new { mensaje = "Cuenta no encontrada." });
        return NoContent();
    }
}