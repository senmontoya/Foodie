﻿using Foodie.Controllers;
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
        public DbSet<combo_promocion> combo_promocion  { get;set; }
        public DbSet<promociones> promociones { get; set; }
        public DbSet<combos> combos {  get; set; }  
        public DbSet<Detalle_Pedido> detalle_Pedido { get; set; }
        public DbSet <menu> menu { get; set; }
        public DbSet <menu_plato> menu_plato { get; set; }
        public DbSet<menu_combo> menu_combo { get; set; }
        public DbSet<Pedido_Local > pedido_Local { get; set; }
        public DbSet<Pedido_Online> pedido_Online { get; set; }
        public DbSet <Platos> plato { get; set; }








        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de relaciones

            //modelBuilder.Entity<Detalle_Pedido>()
            //    .HasOne(dp => dp.PedidoLocal)
            //    .WithMany(pl => pl.DetallePedidos)
            //    .HasForeignKey(dp => dp.encabezado_id)
            //    .HasConstraintName("FK_DetallePedido_PedidoLocal");

            //modelBuilder.Entity<Pedido_Online>()
            //    .HasOne(p => p.Cliente)  // Relación con Cliente
            //    .WithMany(c => c.PedidosOnline)  // Relación inversa
            //    .HasForeignKey(p => p.id_cliente)  // Clave foránea
            //    .HasConstraintName("FK_PedidoOnline_Cliente");

            //modelBuilder.Entity<Carrito>()
            //    .HasOne(c => c.PedidoOnline)
            //    .WithMany(p => p.Carrito)
            //    .HasForeignKey(c => c.plato_id)
            //    .HasConstraintName("FK_Carrito_PedidoOnline");

            //modelBuilder.Entity<Cliente>()
            //.HasOne(c => c.Login_Cliente)
            //.WithOne(p => p.Cliente)
            //.HasForeignKey<Login_Cliente>(l => l.clienteId);


            modelBuilder.Entity<menu_plato>()
                    .HasOne(mp => mp.menu)
                    .WithMany() // Puedes crear una lista en el modelo menu si lo deseas
                    .HasForeignKey(mp => mp.menu_id)
                    .HasConstraintName("FK_menu_plato_menu");

            modelBuilder.Entity<menu_plato>()
                .HasOne(mp => mp.plato)
                .WithMany() // Igualmente, puedes crear una lista en platos si lo necesitas
                .HasForeignKey(mp => mp.plato_id)
                .HasConstraintName("FK_menu_plato_platos");

            base.OnModelCreating(modelBuilder);
        }
    }
}
