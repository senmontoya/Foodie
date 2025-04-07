namespace Foodie.Models
{
    public class ItemPedido
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}
