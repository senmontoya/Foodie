using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodie.Models
{
    public class Login_Cliente
    {
        [Key]
        public int loginid { get; set; }

        [Required] 
        [StringLength(200)] 
        public string correo { get; set; }

        [Required]
        [StringLength(200)] 
        public string contraseña { get; set; }
    }
}
