using Microsoft.EntityFrameworkCore;
using BankLink.Models;

namespace BankLink.Context;

public class BankLinkDbContext : DbContext
{
    public BankLinkDbContext(DbContextOptions<BankLinkDbContext> options) : base(options) { }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Cuenta> Cuentas { get; set; }
    public DbSet<Movimiento> Movimientos { get; set; }
    public DbSet<BancoExterno> BancosExternos { get; set; }
}