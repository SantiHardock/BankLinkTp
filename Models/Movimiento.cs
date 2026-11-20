using System;

namespace BankLink.Models
{
    public class Movimiento
    {
        public int Id { get; set; }
        public string TipoMovimiento { get; set; } // Depósito, Retiro, Transferencia Enviada, Transferencia Recibida
        public decimal Monto { get; set; }
        public DateTime FechaHora { get; set; }
        public string Descripcion { get; set; }

        public int CuentaId { get; set; } // FK
        public Cuenta? Cuenta { get; set; }
    }
}