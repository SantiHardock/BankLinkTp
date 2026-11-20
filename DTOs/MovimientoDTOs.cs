namespace BankLink.DTOs;

public class MovimientoCreateDTO
{
    public int CuentaId { get; set; }
    public decimal Monto { get; set; }
    public string Descripcion { get; set; } = string.Empty;
}