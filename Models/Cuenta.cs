namespace BankLink.Models
{
    public class Cuenta
    {
        public int Id { get; set; }
        public string NumeroCuenta { get; set; }
        public string TipoCuenta { get; set; } // Ej: Ahorro, Corriente
        public decimal Saldo { get; set; }
        public string Estado { get; set; } // Ej: Activa, Inactiva
        
        public int ClienteId { get; set; } // FK
        public Cliente? Cliente { get; set; }
    }
}