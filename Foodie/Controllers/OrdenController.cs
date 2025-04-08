using Microsoft.AspNetCore.Mvc;
using Foodie.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Foodie.Servicios;

namespace Foodie.Controllers
{
    public class OrdenController : Controller
    {
        private readonly FoodieContext _context;

        public OrdenController(FoodieContext context)
        {
            _context = context;
        }

        [Autenticacion]
        public async Task<IActionResult> Index()
        {
            var loginid = HttpContext.Session.GetInt32("loginid");
            if (loginid == null)
            {
                return RedirectToAction("Autenticar", "Login_Clientes");
            }

            var platos = await _context.Platos
                .Where(p => p.estado == 1) // Solo platos activos
                .ToListAsync();

            return View(platos);
        }


        [HttpPost]
        [Autenticacion]
        public IActionResult AgregarAlCarrito(int platoId, int cantidad)
        {
            var plato = _context.Platos.FirstOrDefault(p => p.id == platoId && p.estado == 1);
            if (plato == null || cantidad <= 0)
            {
                TempData["Error"] = "Plato no válido o cantidad inválida.";
                return RedirectToAction("Index");
            }

            // Obtener el carrito de la sesión
            var carritoJson = HttpContext.Session.GetString("Carrito");
            var carrito = string.IsNullOrEmpty(carritoJson)
                ? new List<CarritoItem>()
                : JsonSerializer.Deserialize<List<CarritoItem>>(carritoJson) ?? new List<CarritoItem>();

            // Verificar si el plato ya está en el carrito
            var itemExistente = carrito.FirstOrDefault(i => i.PlatoId == platoId);
            if (itemExistente != null)
            {
                itemExistente.Cantidad += cantidad;
            }
            else
            {
                carrito.Add(new CarritoItem
                {
                    PlatoId = plato.id,
                    Nombre = plato.nombre ?? string.Empty,
                    Precio = plato.precio ?? 0m,
                    Cantidad = cantidad
                });
            }

            // Guardar el carrito actualizado en la sesión
            HttpContext.Session.SetString("Carrito", JsonSerializer.Serialize(carrito));
            TempData["Mensaje"] = $"{plato.nombre} agregado al carrito.";

            return RedirectToAction("Index");
        }
        [Autenticacion]
        public IActionResult VerCarrito()
        {
            var loginid = HttpContext.Session.GetInt32("loginid");
            if (loginid == null)
            {
                return RedirectToAction("Autenticar", "Login_Clientes");
            }

            var carritoJson = HttpContext.Session.GetString("Carrito");
            var carrito = string.IsNullOrEmpty(carritoJson)
                ? new List<CarritoItem>()
                : JsonSerializer.Deserialize<List<CarritoItem>>(carritoJson);

            return View(carrito);
        }

        [HttpPost]
        [Autenticacion]
        public IActionResult EliminarDelCarrito(int platoId)
        {
            var carritoJson = HttpContext.Session.GetString("Carrito");
            var carrito = string.IsNullOrEmpty(carritoJson)
                ? new List<CarritoItem>()
                : JsonSerializer.Deserialize<List<CarritoItem>>(carritoJson);

            var item = carrito.FirstOrDefault(i => i.PlatoId == platoId);
            if (item != null)
            {
                carrito.Remove(item);
                HttpContext.Session.SetString("Carrito", JsonSerializer.Serialize(carrito));
                TempData["Mensaje"] = "Ítem eliminado del carrito.";
            }

            return RedirectToAction("VerCarrito");
        }

        [HttpPost]
        [Autenticacion]
        public IActionResult LimpiarCarrito()
        {
            HttpContext.Session.Remove("Carrito");
            TempData["Mensaje"] = "Carrito vaciado.";
            return RedirectToAction("VerCarrito");
        }

        [HttpPost]
        [Autenticacion]
        public async Task<IActionResult> ConfirmarPedido()
        {
            var loginid = HttpContext.Session.GetInt32("loginid");
            if (loginid == null)
            {
                return RedirectToAction("Autenticar", "Login_Clientes");
            }

            var cliente = await _context.Cliente.FirstOrDefaultAsync(c => c.loginid == loginid);
            if (cliente == null)
            {
                return RedirectToAction("Autenticar", "Login_Clientes");
            }

            // Obtener el carrito de la sesión
            var carritoJson = HttpContext.Session.GetString("Carrito");
            var carrito = string.IsNullOrEmpty(carritoJson)
                ? new List<CarritoItem>()
                : JsonSerializer.Deserialize<List<CarritoItem>>(carritoJson) ?? new List<CarritoItem>();

            if (!carrito.Any())
            {
                TempData["Error"] = "El carrito está vacío.";
                return RedirectToAction("VerCarrito");
            }

            // Crear el pedido en Pedido_Online
            var pedido = new Pedido_Online
            {
                cliente_id = cliente.clienteId,
                fechaApertura = DateTime.Now,
                estado = "Abierta"
            };
            _context.Pedido_Online.Add(pedido);
            await _context.SaveChangesAsync(); // Guardar para obtener el id_pedido

            // Guardar los detalles en detalle_pedido_online y historial_pedido
            foreach (var item in carrito)
            {
                var detalle = new Detalle_Pedido_Online
                {
                    pedido_id = pedido.id_pedido,
                    tipo_item = "Plato",
                    item_id = item.PlatoId,
                    cantidad = item.Cantidad,
                    subtotal = item.Subtotal
                };
                _context.Detalle_Pedido_Online.Add(detalle);

                var historial = new Historial_Pedido
                {
                    pedido_id = pedido.id_pedido,
                    item_id = item.PlatoId,
                    tipo_item = "Plato",
                    cantidad = item.Cantidad,
                    estado = "Pendiente",
                    fecha_venta = DateTime.Now
                };
                _context.Historial_Pedido.Add(historial);
            }

            await _context.SaveChangesAsync();

            // Limpiar el carrito
            HttpContext.Session.Remove("Carrito");

            // Redirigir a la vista de confirmación
            return RedirectToAction("Confirmacion", new { idPedido = pedido.id_pedido });
        }

        [Autenticacion]
        public async Task<IActionResult> Confirmacion(int idPedido)
        {
            var loginid = HttpContext.Session.GetInt32("loginid");
            if (loginid == null)
            {
                return RedirectToAction("Autenticar", "Login_Clientes");
            }

            var pedido = await _context.Pedido_Online
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(p => p.id_pedido == idPedido);

            if (pedido == null || pedido.cliente_id != (await _context.Cliente.FirstOrDefaultAsync(c => c.loginid == loginid))?.clienteId)
            {
                return NotFound();
            }

            var detalles = await _context.Detalle_Pedido_Online
                .Include(d => d.Plato)
                .Where(d => d.pedido_id == idPedido)
                .ToListAsync();

            ViewBag.Pedido = pedido;
            ViewBag.Nombre = pedido.Cliente?.nombre;
            return View(detalles);
        }

        [Autenticacion]
        public async Task<IActionResult> Historial()
        {
            var loginid = HttpContext.Session.GetInt32("loginid");
            if (loginid == null)
            {
                return RedirectToAction("Autenticar", "Login_Clientes");
            }

            var cliente = await _context.Cliente.FirstOrDefaultAsync(c => c.loginid == loginid);
            if (cliente == null)
            {
                return RedirectToAction("Autenticar", "Login_Clientes");
            }

            ViewBag.Nombre = cliente.nombre;

            var historial = await _context.Historial_Pedido
                .Include(h => h.Pedido)
                .Include(h => h.Plato)
                .Where(h => h.Pedido.cliente_id == cliente.clienteId)
                .ToListAsync();

            return View(historial);
        }
    }
}