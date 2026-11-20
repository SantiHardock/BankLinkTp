using BankLink.Models;
using BankLink.DTOs;

namespace BankLink.Interfaces;

public interface IClienteService
{
    Task<List<Cliente>> GetAll();
    Task<Cliente?> GetById(int id);
    Task<Cliente> Create(ClienteCreateDTO nuevoCliente);
    Task<bool> Update(int id, ClienteUpdateDTO clienteActualizado);
    Task<bool> Delete(int id);
}