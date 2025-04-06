using System.ComponentModel.DataAnnotations;

namespace Foodie.Models
{
    public class Login_Cliente
    {
        [Key]
        public int loginid { get; set; }
        public string? correo { get; set; } 
        public string? contraseña { get; set; }
    }
}
