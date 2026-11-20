using Microsoft.AspNetCore.Mvc;
using BankLink.Interfaces;
using BankLink.DTOs;
using BankLink.Filters; 

namespace BankLink.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransferenciasController : ControllerBase
{
    private readonly ITransferenciaService _transferenciaService;

    public TransferenciasController(ITransferenciaService transferenciaService)
    {
        _transferenciaService = transferenciaService;
    }

    // Endpoint interno: Nuestros clientes lo usan para enviar dinero. No requiere API Key.
    [HttpPost("enviar")]
    public async Task<IActionResult> Enviar([FromBody] TransferenciaOutboundDTO dto)
    {
        if (dto.Monto <= 0) return BadRequest(new { mensaje = "El monto a transferir debe ser mayor a cero." });

        var exito = await _transferenciaService.EnviarTransferencia(dto);
        
        if (!exito) return BadRequest(new { mensaje = "La transferencia falló. Verifique el saldo, el estado de la cuenta o la conexión con el banco destino." });

        return Ok(new { mensaje = "Transferencia interbancaria enviada con éxito." });
    }

    // Endpoint externo: Protegido por el candado [ApiKey]. Los otros grupos nos consumen por acá.
    [ApiKey] 
    [HttpPost("recibir")]
    public async Task<IActionResult> Recibir([FromBody] TransferenciaInboundDTO dto)
    {
        if (dto.Monto <= 0) return BadRequest(new { mensaje = "El monto recibido debe ser mayor a cero." });

        var exito = await _transferenciaService.RecibirTransferencia(dto);

        if (!exito) return NotFound(new { mensaje = "Error: La cuenta destino no existe o se encuentra inactiva en BankLink." });

        return Ok(new { mensaje = "Transferencia recibida y acreditada con éxito en BankLink." });
    }
}