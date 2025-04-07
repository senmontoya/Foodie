namespace Foodie.Models.ViewModels
{
    public class ConfirmarPedidoViewModel
    {
        public List<ItemPedido> Items { get; set; }
        public decimal SubTotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }
        public string NombreCliente { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }
    }
}
