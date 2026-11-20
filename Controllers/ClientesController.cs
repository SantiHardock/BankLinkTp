using Microsoft.AspNetCore.Mvc;
using BankLink.Models;
using BankLink.Interfaces;
using BankLink.DTOs;

namespace BankLink.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;

    public ClientesController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cliente>>> GetAll()
    {
        return Ok(await _clienteService.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cliente>> GetById(int id)
    {
        var cliente = await _clienteService.GetById(id);
        if (cliente == null) return NotFound(new { mensaje = "Cliente no encontrado" });
        return Ok(cliente);
    }

    [HttpPost]
    public async Task<ActionResult<Cliente>> Create([FromBody] ClienteCreateDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.DNI) || string.IsNullOrWhiteSpace(dto.Nombre))
            return BadRequest(new { mensaje = "El DNI y Nombre son obligatorios." });

        var creado = await _clienteService.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ClienteUpdateDTO dto)
    {
        var actualizado = await _clienteService.Update(id, dto);
        if (!actualizado) return NotFound(new { mensaje = "Cliente no encontrado para actualizar." });
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _clienteService.Delete(id);
        if (!eliminado) return NotFound(new { mensaje = "Cliente no encontrado para eliminar." });
        return NoContent();
    }
}