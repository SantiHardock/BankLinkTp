namespace BankLink.Models;

public class BancoExterno
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string UrlApi { get; set; } = string.Empty;
}