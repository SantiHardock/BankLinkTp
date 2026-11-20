namespace BankLink.DTOs;

public class CuentaCreateDTO
{
    public string NumeroCuenta { get; set; } = string.Empty;
    public string TipoCuenta { get; set; } = string.Empty; // Ej: Ahorro, Corriente
    public decimal SaldoInicial { get; set; }
    public int ClienteId { get; set; }
}

public class CuentaUpdateDTO
{
    public string TipoCuenta { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty; // Ej: Activa, Inactiva
}