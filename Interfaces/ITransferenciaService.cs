using BankLink.DTOs;

namespace BankLink.Interfaces;

public interface ITransferenciaService
{
    Task<bool> EnviarTransferencia(TransferenciaOutboundDTO dto);
    Task<bool> RecibirTransferencia(TransferenciaInboundDTO dto);
}