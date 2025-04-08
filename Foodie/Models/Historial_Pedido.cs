using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodie.Models
{
    [Table("historial_pedido")] // Explicitly map to the table name
    public class Historial_Pedido
    {
        [Key]
        [Column("historial_id")]
        public int historial_id { get; set; }

        [Column("pedido_id")]
        [ForeignKey("Pedido")]
        [Required]
        public int pedido_id { get; set; }

        [Column("item_id")]
        [ForeignKey("Plato")]
        [Required]
        public int item_id { get; set; }

        [Column("tipo_item")]
        [StringLength(10)]
        [Required]
        public string tipo_item { get; set; }

        [Column("cantidad")]
        [Required]
        public int cantidad { get; set; }

        [Column("estado")]
        [StringLength(20)]
        [Required]
        public string estado { get; set; }

        [Column("fecha_venta", TypeName = "datetime")]
        public DateTime? fecha_venta { get; set; }

        public virtual Pedido_Online Pedido { get; set; }
        public virtual Platos Plato { get; set; }

        [NotMapped]
        public decimal? Total => tipo_item == "Plato" && Plato != null ? cantidad * Plato.precio : null;
    }
}