using Foodie.Controllers;
using Microsoft.EntityFrameworkCore;

namespace Foodie.Models
{
    public class FoodieContext : DbContext
    {
        public FoodieContext(DbContextOptions<FoodieContext> options) : base(options)
        {

        }

        public DbSet<Login_Cliente> Login_Cliente { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Platos> Platos { get; set; }
        public DbSet<Carrito> Carrito { get; set; }
        public DbSet<combo_promocion> combo_Promocion  { get;set; }
        public DbSet<combos> combos {  get; set; }  
        public DbSet<Detalle_Pedido> detalle_Pedido { get; set; }
        public DbSet <menu> menu { get; set; }
        public DbSet <menu_plato> menu_plato { get; set; }
        public DbSet<Pedido_Local > pedido_Local { get; set; }
        public DbSet<Pedido_Online> pedido_Online { get; set; }
        public DbSet <Platos> plato { get; set; }
    }
}
