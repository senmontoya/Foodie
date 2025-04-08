using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodie.Models
{
    [Table("detalle_pedido_online")]
    public class Detalle_Pedido_Online
    {
        [Key]
        [Column("id_detalle")]
        public int id_detalle { get; set; }

        [Column("pedido_id")]
        [ForeignKey("Pedido_Online")]
        [Required]
        public int pedido_id { get; set; }

        [Column("tipo_item")]
        [StringLength(10)]
        [Required]
        public string tipo_item { get; set; }

        [Column("item_id")]
        [ForeignKey("Plato")]
        [Required]
        public int item_id { get; set; }

        [Column("cantidad")]
        [Required]
        public int cantidad { get; set; }

        [Column("subtotal")]
        [Required]
        public decimal subtotal { get; set; }

        public virtual Pedido_Online Pedido_Online { get; set; }
        public virtual Platos Plato { get; set; }
    }
}