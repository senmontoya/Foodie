using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodie.Models
{
    public class Cliente
    {
        [Key]
        public int clienteId { get; set; }
        public string? nombre { get; set; }
        public string? telefono { get; set; }
        public string? direccion { get; set; }
        public decimal? latitud { get; set; }
        public decimal? longitud { get; set; }

        [ForeignKey("loginid")]
        public Login_Cliente login_Cliente { get; set; }
    }
}
