using Microsoft.AspNetCore.Mvc;
using Foodie.Models;
using Microsoft.EntityFrameworkCore;
using Foodie.Servicios;
using Microsoft.AspNetCore.Http;

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
        public async Task<IActionResult> Historial()
        {
            var loginid = HttpContext.Session.GetInt32("loginid");
            if (loginid == null)
            {
                return RedirectToAction("Autenticar", "Login_Clientes");
            }

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(c => c.loginid == loginid);

            if (cliente != null)
            {
                ViewBag.Nombre = cliente.nombre;
            }
            else
            {
                ViewBag.Nombre = "Invitado";
            }

            var historial = await _context.Historial_Pedido
                .Include(h => h.Plato)
                .Join(_context.Historial_Pedido,
                      h => h.historial_id,
                      v => v.historial_id,
                      (h, v) => new { Historial = h, Venta = v })
                //.Where(hv => hv.Venta.historial_id == )
                .Select(hv => hv.Historial)
                .ToListAsync();

            return View(historial);
        }
    }
}