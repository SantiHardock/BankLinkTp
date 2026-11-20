namespace BankLink.DTOs;

public class TransferenciaOutboundDTO
{
    public int MiCuentaId { get; set; } // La cuenta de tu cliente que envía
    public int BancoDestinoId { get; set; } // El ID del banco de tus compañeros
    public string CuentaDestino { get; set; } = string.Empty; // El número de cuenta del cliente de ellos
    public decimal Monto { get; set; }
}

public class TransferenciaInboundDTO
{
    public int NumeroCuentaDestino { get; set; }
    public string CodigoBancoOrigen { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public string Descripcion { get; set; } = string.Empty;
}