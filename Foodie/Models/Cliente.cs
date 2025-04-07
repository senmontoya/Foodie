using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodie.Models
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int clienteId { get; set; }

        [Required]
        [StringLength(200)]
        public string nombre { get; set; }

        [Required]
        [StringLength(200)]
        public string telefono { get; set; }

        [Required]
        [StringLength(200)]
        public string direccion { get; set; }

        [Required] 
        [Column(TypeName = "DECIMAL(10,6)")]
        public decimal latitud { get; set; }

        [Required]
        [Column(TypeName = "DECIMAL(10,6)")]
        public decimal longitud { get; set; }

        [ForeignKey("login_Cliente")]
        public int loginid { get; set; } 

        public virtual Login_Cliente login_Cliente { get; set; }
    }
}
