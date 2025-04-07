using System.ComponentModel.DataAnnotations;

namespace Foodie.Models
{
    public class menu_plato
    {
        [Key]
        public int id { get; set; }
        public int? menu_id { get; set; }
        public int? plato_id { get; set; }
        public int estado { get; set; }


        public menu menu { get; set; }
        public Platos plato { get; set; }
    }
}
