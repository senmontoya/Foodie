using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodie.Models
{
    [Table("Pedido_Online")]
    public class Pedido_Online
    {
        [Key]
        [Column("id_pedido")]
        public int id_pedido { get; set; }

        [Column("cliente_id")]
        [ForeignKey("Cliente")]
        public int? cliente_id { get; set; }

        [Column("fechaApertura", TypeName = "datetime")]
        public DateTime? fechaApertura { get; set; }

        [Column("estado")]
        [StringLength(20)]
        [Required]
        public string estado { get; set; }

        public virtual Cliente Cliente { get; set; }
    }
}