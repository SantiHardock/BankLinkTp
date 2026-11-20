using BankLink.Models;
using BankLink.DTOs;
using BankLink.Interfaces;
using BankLink.Context;
using Microsoft.EntityFrameworkCore;

namespace BankLink.Services;

public class CuentaDbService(BankLinkDbContext context) : ICuentaService
{
    private readonly BankLinkDbContext _context = context;

    public async Task<List<Cuenta>> GetAll()
    {
        // Include(c => c.Cliente) trae los datos del cliente dueño de la cuenta
        return await _context.Cuentas.Include(c => c.Cliente).ToListAsync();
    }

    public async Task<Cuenta?> GetById(int id)
    {
        return await _context.Cuentas.Include(c => c.Cliente).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Cuenta?> Create(CuentaCreateDTO dto)
    {
        // Validar que el cliente exista antes de asignarle una cuenta
        var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == dto.ClienteId);
        if (!clienteExiste) return null;

        var cuenta = new Cuenta
        {
            NumeroCuenta = dto.NumeroCuenta,
            TipoCuenta = dto.TipoCuenta,
            Saldo = dto.SaldoInicial,
            Estado = "Activa", // Toda cuenta nueva nace activa
            ClienteId = dto.ClienteId
        };

        _context.Cuentas.Add(cuenta);
        await _context.SaveChangesAsync();
        return cuenta;
    }

    public async Task<bool> Update(int id, CuentaUpdateDTO dto)
    {
        var cuenta = await _context.Cuentas.FindAsync(id);
        if (cuenta == null) return false;

        cuenta.TipoCuenta = dto.TipoCuenta;
        cuenta.Estado = dto.Estado;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var cuenta = await _context.Cuentas.FindAsync(id);
        if (cuenta == null) return false;

        _context.Cuentas.Remove(cuenta);
        await _context.SaveChangesAsync();
        return true;
    }
}