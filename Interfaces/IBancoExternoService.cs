using BankLink.Models;
using BankLink.DTOs;

namespace BankLink.Interfaces;

public interface IBancoExternoService
{
    Task<List<BancoExterno>> GetAll();
    Task<BancoExterno?> GetById(int id);
    Task<BancoExterno> Create(BancoExternoCreateDTO dto);
    Task<bool> Update(int id, BancoExternoUpdateDTO dto);
    Task<bool> Delete(int id);
}