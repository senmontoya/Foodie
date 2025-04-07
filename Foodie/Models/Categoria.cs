using System.ComponentModel.DataAnnotations;

namespace Foodie.Models
{
    public class Categoria
    {
        [Key]
        public int id { get; set; }
        public string? nombre { get; set; }
        public int? estado { get; set; } 
    }
}
