﻿namespace Foodie.Models
{
    public class CarritoItem
    {
        public int PlatoId { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal => Precio * Cantidad;
    }
}
