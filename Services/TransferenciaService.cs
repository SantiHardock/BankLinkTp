using BankLink.Models;
using BankLink.DTOs;
using BankLink.Interfaces;
using BankLink.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text;

namespace BankLink.Services;

public class TransferenciaService(BankLinkDbContext context, HttpClient httpClient) : ITransferenciaService
{
    private readonly BankLinkDbContext _context = context;
    private readonly HttpClient _httpClient = httpClient;

    private const string ApiKeyCompaneros = "mi-clave-secreta-de-prueba123";

    public async Task<bool> EnviarTransferencia(TransferenciaOutboundDTO dto)
    {
        try
        {
            var miCuenta = await _context.Cuentas.FindAsync(dto.MiCuentaId);
            var bancoDestino = await _context.BancosExternos.FindAsync(dto.BancoDestinoId);

            if (miCuenta == null || bancoDestino == null || miCuenta.Saldo < dto.Monto || miCuenta.Estado != "Activa")
                return false;

            // Cuerpo según la guía de conexión
            var payload = new
            {
                numeroCuentaDestino = int.TryParse(dto.CuentaDestino, out var numDestino) ? numDestino : 0,
                codigoBancoOrigen = "BANKLINK",
                monto = dto.Monto,
                descripcion = $"Transferencia desde BankLink ({miCuenta.Id})"
            };

            var json = JsonSerializer.Serialize(payload);
            var urlDestino = bancoDestino.UrlApi.TrimEnd('/');
            if (!urlDestino.EndsWith("/api/transferencias/recibir", StringComparison.OrdinalIgnoreCase))
            {
                urlDestino = $"{urlDestino}/api/transferencias/recibir";
            }

            var request = new HttpRequestMessage(HttpMethod.Post, urlDestino)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            request.Headers.Remove("X-API-Key");
            request.Headers.Add("X-API-Key", ApiKeyCompaneros);

            var response = await _httpClient.SendAsync(request);
            Console.WriteLine($"Enviando transferencia a {urlDestino}, StatusCode: {response.StatusCode}");
            if (!response.IsSuccessStatusCode)
                return false;

            miCuenta.Saldo -= dto.Monto;

            var movimiento = new Movimiento
            {
                CuentaId = miCuenta.Id,
                TipoMovimiento = "Transferencia Enviada",
                Monto = dto.Monto,
                FechaHora = DateTime.Now,
                Descripcion = $"Hacia cuenta {dto.CuentaDestino} en {bancoDestino.Nombre}"
            };

            _context.Movimientos.Add(movimiento);
            await _context.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RecibirTransferencia(TransferenciaInboundDTO dto)
    {
        // Buscamos la cuenta por número de cuenta (si tu modelo usa NumeroCuenta como string, ajustá esta comparación)
        var cuentaStr = dto.NumeroCuentaDestino.ToString();
        var miCuenta = await _context.Cuentas
            .FirstOrDefaultAsync(c => c.NumeroCuenta == cuentaStr || c.Id == dto.NumeroCuentaDestino);

        if (miCuenta == null || miCuenta.Estado != "Activa")
            return false;

        miCuenta.Saldo += dto.Monto;

        var movimiento = new Movimiento
        {
            CuentaId = miCuenta.Id,
            TipoMovimiento = "Transferencia Recibida",
            Monto = dto.Monto,
            FechaHora = DateTime.Now,
            Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion)
                ? $"Transferencia entrante desde: {dto.CodigoBancoOrigen}"
                : $"{dto.Descripcion} (Origen: {dto.CodigoBancoOrigen})"
        };

        _context.Movimientos.Add(movimiento);
        await _context.SaveChangesAsync();

        return true;
    }
}