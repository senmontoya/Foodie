using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodie.Models
{
    public class Platos
    {
        [Key]
        public int id { get; set; }
        public string? nombre { get; set; }
        public string? descripcion { get; set; }
        public decimal? precio { get; set; }
        public string? imagen { get; set; }
        [ForeignKey("categoria_id")]
        public Categoria categoria { get; set; }
        public int? estado { get; set; }

    }
}
