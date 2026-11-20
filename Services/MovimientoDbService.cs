using BankLink.Models;
using BankLink.DTOs;
using BankLink.Interfaces;
using BankLink.Context;
using Microsoft.EntityFrameworkCore;

namespace BankLink.Services;

public class MovimientoDbService(BankLinkDbContext context) : IMovimientoService
{
    private readonly BankLinkDbContext _context = context;

    public async Task<List<Movimiento>> GetPorCuenta(int cuentaId)
    {
        return await _context.Movimientos
            .Where(m => m.CuentaId == cuentaId)
            .OrderByDescending(m => m.FechaHora)
            .ToListAsync();
    }

    public async Task<Movimiento?> RealizarDeposito(MovimientoCreateDTO dto)
    {
        var cuenta = await _context.Cuentas.FindAsync(dto.CuentaId);
        if (cuenta == null || cuenta.Estado != "Activa") return null;

        // Actualizamos el saldo
        cuenta.Saldo += dto.Monto;

        // Creamos el registro del movimiento
        var movimiento = new Movimiento
        {
            CuentaId = dto.CuentaId,
            TipoMovimiento = "Depósito",
            Monto = dto.Monto,
            FechaHora = DateTime.Now,
            Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? "Depósito en sucursal" : dto.Descripcion
        };

        _context.Movimientos.Add(movimiento);
        await _context.SaveChangesAsync(); 
        
        return movimiento;
    }

    public async Task<Movimiento?> RealizarRetiro(MovimientoCreateDTO dto)
    {
        var cuenta = await _context.Cuentas.FindAsync(dto.CuentaId);
        
        // Validación extra: el saldo debe ser mayor o igual al monto solicitado
        if (cuenta == null || cuenta.Estado != "Activa" || cuenta.Saldo < dto.Monto) return null;

        // Descontamos el saldo
        cuenta.Saldo -= dto.Monto;

        var movimiento = new Movimiento
        {
            CuentaId = dto.CuentaId,
            TipoMovimiento = "Retiro",
            Monto = dto.Monto,
            FechaHora = DateTime.Now,
            Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? "Retiro en sucursal" : dto.Descripcion
        };

        _context.Movimientos.Add(movimiento);
        await _context.SaveChangesAsync();
        
        return movimiento;
    }
}