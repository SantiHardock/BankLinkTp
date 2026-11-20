using BankLink.Models;
using BankLink.DTOs;

namespace BankLink.Interfaces;

public interface ICuentaService
{
    Task<List<Cuenta>> GetAll();
    Task<Cuenta?> GetById(int id);
    Task<Cuenta?> Create(CuentaCreateDTO dto);
    Task<bool> Update(int id, CuentaUpdateDTO dto);
    Task<bool> Delete(int id);
}