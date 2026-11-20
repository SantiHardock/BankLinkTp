using Microsoft.AspNetCore.Mvc;
using BankLink.Models;
using BankLink.Interfaces;
using BankLink.DTOs;

namespace BankLink.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovimientosController : ControllerBase
{
    private readonly IMovimientoService _movimientoService;

    public MovimientosController(IMovimientoService movimientoService)
    {
        _movimientoService = movimientoService;
    }

    [HttpGet("cuenta/{cuentaId}")]
    public async Task<ActionResult<IEnumerable<Movimiento>>> GetHistorial(int cuentaId)
    {
        return Ok(await _movimientoService.GetPorCuenta(cuentaId));
    }

    [HttpPost("deposito")]
    public async Task<ActionResult<Movimiento>> Deposito([FromBody] MovimientoCreateDTO dto)
    {
        if (dto.Monto <= 0) return BadRequest(new { mensaje = "El monto debe ser mayor a cero." });

        var movimiento = await _movimientoService.RealizarDeposito(dto);
        if (movimiento == null) return BadRequest(new { mensaje = "Cuenta inactiva o no encontrada." });

        return Ok(movimiento);
    }

    [HttpPost("retiro")]
    public async Task<ActionResult<Movimiento>> Retiro([FromBody] MovimientoCreateDTO dto)
    {
        if (dto.Monto <= 0) return BadRequest(new { mensaje = "El monto debe ser mayor a cero." });

        var movimiento = await _movimientoService.RealizarRetiro(dto);
        if (movimiento == null) return BadRequest(new { mensaje = "Cuenta no encontrada, inactiva, o saldo insuficiente." });

        return Ok(movimiento);
    }
}