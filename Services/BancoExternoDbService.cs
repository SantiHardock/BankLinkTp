using BankLink.Models;
using BankLink.DTOs;
using BankLink.Interfaces;
using BankLink.Context;
using Microsoft.EntityFrameworkCore;

namespace BankLink.Services;

public class BancoExternoDbService(BankLinkDbContext context) : IBancoExternoService
{
    private readonly BankLinkDbContext _context = context;

    public async Task<List<BancoExterno>> GetAll()
    {
        return await _context.BancosExternos.ToListAsync();
    }

    public async Task<BancoExterno?> GetById(int id)
    {
        return await _context.BancosExternos.FindAsync(id);
    }

    public async Task<BancoExterno> Create(BancoExternoCreateDTO dto)
    {
        var banco = new BancoExterno
        {
            Nombre = dto.Nombre,
            UrlApi = dto.UrlApi
        };

        _context.BancosExternos.Add(banco);
        await _context.SaveChangesAsync();
        return banco;
    }

    public async Task<bool> Update(int id, BancoExternoUpdateDTO dto)
    {
        var banco = await _context.BancosExternos.FindAsync(id);
        if (banco == null) return false;

        banco.Nombre = dto.Nombre;
        banco.UrlApi = dto.UrlApi;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var banco = await _context.BancosExternos.FindAsync(id);
        if (banco == null) return false;

        _context.BancosExternos.Remove(banco);
        await _context.SaveChangesAsync();
        return true;
    }
}