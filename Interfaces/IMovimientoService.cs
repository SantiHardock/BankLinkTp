using BankLink.Models;
using BankLink.DTOs;

namespace BankLink.Interfaces;

public interface IMovimientoService
{
    Task<List<Movimiento>> GetPorCuenta(int cuentaId);
    Task<Movimiento?> RealizarDeposito(MovimientoCreateDTO dto);
    Task<Movimiento?> RealizarRetiro(MovimientoCreateDTO dto);
}