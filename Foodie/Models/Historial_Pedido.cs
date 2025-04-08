using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodie.Models
{
    public class Historial_Pedido
    {
        [Key]
        public int? historial_id { get; set; }
        public string? tipoItem { get; set; }
        public int? cantidad { get; set; }
        public decimal? total { get; set; }
        public string? estado { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? fecha_venta { get; set; }

        [ForeignKey("palto_id")]
        public Platos Plato { get; set; }
       
        

    }

}
