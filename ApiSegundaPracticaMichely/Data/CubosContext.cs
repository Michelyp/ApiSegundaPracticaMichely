using ApiSegundaPracticaMichely.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiSegundaPracticaMichely.Data
{
    public class CubosContext : DbContext
    {
        public CubosContext(DbContextOptions<CubosContext> options)
       : base(options) { }
        public DbSet<Cubo> Cubos { get; set; }
        public DbSet<UsuariosCubo> Usuarios { get; set; }
        public DbSet<ComprasCubo> Compras { get; set; }
    }
}
