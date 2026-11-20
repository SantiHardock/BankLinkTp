using BankLink.Models;
using BankLink.DTOs;
using BankLink.Interfaces;
using BankLink.Context;
using Microsoft.EntityFrameworkCore;

namespace BankLink.Services;

public class ClienteDbService(BankLinkDbContext context) : IClienteService
{
    private readonly BankLinkDbContext _context = context;

    public async Task<List<Cliente>> GetAll()
    {
        return await _context.Clientes.ToListAsync();
    }

    public async Task<Cliente?> GetById(int id)
    {
        return await _context.Clientes.FindAsync(id);
    }

    public async Task<Cliente> Create(ClienteCreateDTO dto)
    {
        var cliente = new Cliente
        {
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            DNI = dto.DNI,
            Direccion = dto.Direccion,
            Telefono = dto.Telefono,
            Email = dto.Email
        };

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return cliente;
    }

    public async Task<bool> Update(int id, ClienteUpdateDTO dto)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null) return false;

        cliente.Nombre = dto.Nombre;
        cliente.Apellido = dto.Apellido;
        cliente.Direccion = dto.Direccion;
        cliente.Telefono = dto.Telefono;
        cliente.Email = dto.Email;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null) return false;

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();
        return true;
    }
}