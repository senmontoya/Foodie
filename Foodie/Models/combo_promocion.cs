﻿using System.ComponentModel.DataAnnotations;

namespace Foodie.Models
{
    public class combo_promocion
    {
        [Key]
        public int id { get; set; }
        public int? combo_id { get; set; }
        public int? promocion_id { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime fecha_fin { get; set; }
        public int estado { get; set; }
    }
}
