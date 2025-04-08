using Foodie.Controllers;
using Microsoft.EntityFrameworkCore;

namespace Foodie.Models
{
    public class FoodieContext : DbContext
    {
        public FoodieContext(DbContextOptions<FoodieContext> options) : base(options)
        {

        }

        public DbSet<Historial_Pedido> Historial_Pedido { get; set; }
        public DbSet<Pedido_Online> Pedido_Online { get; set; }
        public DbSet<Platos> Platos { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Login_Cliente> Login_Cliente { get; set; }
        public DbSet<Detalle_Pedido_Online> Detalle_Pedido_Online { get; set; }

    }
}
