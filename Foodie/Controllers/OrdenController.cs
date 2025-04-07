using Foodie.Helpers;
using Foodie.Models;
using Foodie.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ProyectoVentas.Controllers
{
    public class OrdenController : Controller
    {
        private readonly FoodieContext _context;

        public OrdenController(FoodieContext restauranteDBContext)
        {
            _context = restauranteDBContext;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Orden");
        }

        public IActionResult Orden()
        {
            int clienteId = 1;//Estatico hasta que login funcione.

            var ordenes = _context.pedido_Online
                .Where(p => p.id_pedido == clienteId)
                .Include(p => p.Carrito)
                    .ThenInclude(c => c.Plato)
                .Include(p => p.Carrito)
                    .ThenInclude(c => c.Combo)
                .AsEnumerable()
                .Select(p => new OrdenViewModel
                {
                    NumRecibo = "OL" + p.id_pedido.ToString("D6"),
                    Estado = p.estado,
                    FechaPedido = p.fecha_pedido,
                    Items = p.Carrito
                        .Select(d => new ItemOrdenViewModel
                        {
                            Nombre = d.Plato != null ? d.Plato.nombre : (d.Combo != null ? d.Combo.nombre : "Desconocido"),
                            Cantidad = d.cantidad,
                            Precio = (decimal)(d.Plato != null ? d.Plato.precio : (d.Combo != null ? d.Combo.precio : 0)),
                            Total = d.total,
                            Descripcion = d.Plato != null ? d.Plato.descripcion : (d.Combo != null ? d.Combo.descripcion : "Sin descripción"),
                            ImagenUrl = d.Plato != null ? d.Plato.imagen : "~/img/combo-icon.png"  // Imagen por defecto para combos
                        })
                        .ToList(),
                    Total = p.Carrito.Sum(d => (decimal?)d.total) ?? 0
                })
                .ToList();

            return View(ordenes);
        }

        public IActionResult Details(int id)
        {
            if (id == 0)
                return NotFound();

            var pedido = _context.pedido_Online
                .Include(p => p.Carrito)
                    .ThenInclude(c => c.Plato)
                .Include(p => p.Carrito)
                    .ThenInclude(c => c.Combo)
                .FirstOrDefault(p => p.id_pedido == id);

            if (pedido == null)
                return NotFound();

            var model = new OrdenViewModel
            {
                NumRecibo = "OL" + pedido.id_pedido.ToString("D6"),
                Estado = pedido.estado,
                FechaPedido = pedido.fecha_pedido,
                Items = pedido.Carrito
                    .Select(d => new ItemOrdenViewModel
                    {
                        Nombre = d.Plato != null ? d.Plato.nombre : (d.Combo != null ? d.Combo.nombre : "Desconocido"),
                        Cantidad = d.cantidad,
                        Precio = (decimal)(d.Plato != null ? d.Plato.precio : (d.Combo != null ? d.Combo.precio : 0)),
                        Total = d.total,
                        Descripcion = d.Plato != null ? d.Plato.descripcion : (d.Combo != null ? d.Combo.descripcion : "Sin descripción"),
                        ImagenUrl = d.Plato != null ? d.Plato.imagen : "~/img/combo-icon.png"
                    })
                    .ToList(),
                Total = pedido.Carrito.Sum(d => (decimal?)d.total) ?? 0,
                IdPedido = pedido.id_pedido
            };

            return View(model);
        }

        public IActionResult AgregarAlPedido(int productoId)
        {
            var carrito = HttpContext.Session.GetObjectFromJson<List<ItemPedido>>("Carrito") ?? new List<ItemPedido>();

            var plato = _context.Platos.FirstOrDefault(p => p.id == productoId);

            if (plato != null)
            {
                carrito.Add(new ItemPedido
                {
                    ProductoId = plato.id,
                    Nombre = plato.nombre,
                    Descripcion = plato.descripcion,
                    Precio = plato.precio,
                    Cantidad = 1
                });
            }
            else
            {
                var combo = _context.combos.FirstOrDefault(c => c.id == productoId);
                if (combo != null)
                {
                    carrito.Add(new ItemPedido
                    {
                        ProductoId = combo.id,
                        Nombre = combo.nombre,
                        Descripcion = combo.descripcion,
                        Precio = combo.precio,
                        Cantidad = 1
                    });
                }
                else
                {
                    return RedirectToAction("Welcome", "Welcome");


                }
            }

            HttpContext.Session.SetObjectAsJson("Carrito", carrito);

            return RedirectToAction("Welcome", "Welcome");
        }

        public IActionResult LimpiarPedido()
        {
            HttpContext.Session.Remove("Carrito");
            return RedirectToAction("Welcome", "Welcome");
        }

        
        public IActionResult ConfirmarPedido()
        {

            var carrito = HttpContext.Session.GetObjectFromJson<List<ItemPedido>>("Carrito") ?? new List<ItemPedido>();

            decimal subTotal = carrito.Sum(item => item.Precio * item.Cantidad);
            decimal iva = subTotal * 0.13m; 
            decimal total = subTotal + iva;

            var loginid = HttpContext.Session.GetInt32("loginid");

            if (loginid == null)
            {
                return RedirectToAction("Autenticar", "Login_Clientes");
            }

            var cliente = _context.Cliente.FirstOrDefault(c => c.login_Cliente.loginid == loginid);

            if (cliente == null)
            {
                return RedirectToAction("Autenticar", "Login_Clientes");
            }

            var loginCliente = _context.Login_Cliente.FirstOrDefault(lc => lc.loginid == loginid);

            var viewModel = new ConfirmarPedidoViewModel
            {
                Items = carrito,
                SubTotal = subTotal,
                IVA = iva,
                Total = total,
                NombreCliente = cliente?.nombre,
                Telefono = cliente?.telefono,
                Correo = loginCliente?.correo,
                Direccion = cliente?.direccion
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ConfirmarPedidoPost()
        {
            var carrito = HttpContext.Session.GetObjectFromJson<List<ItemPedido>>("Carrito");
            if (carrito == null || !carrito.Any())
            {

                return RedirectToAction("Welcome", "Welcome");
            }

            // Aquí guardas el pedido y los detalles en la base de datos (puedo ayudarte con eso si quieres)

            HttpContext.Session.Remove("Carrito");

            return RedirectToAction("Welcome", "Welcome");
        }
    }
}
