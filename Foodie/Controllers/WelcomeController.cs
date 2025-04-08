using Foodie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Foodie.Controllers
{
    public class WelcomeController : Controller
    {
        private readonly FoodieContext foodieContext_;

        public WelcomeController(FoodieContext context)
        {
            foodieContext_ = context;
        }
        [AllowAnonymous]
        public IActionResult Welcome()
        {
            var platos = foodieContext_.Platos
            .Where(p => p.estado == 1)
            .ToList();

            if (platos != null)
            {
                ViewBag.platosImagenes = platos.Select(p => p.imagen).ToList();
                ViewBag.descripcion = platos.Select(p => p.descripcion).ToList();
                ViewBag.nombre = platos.Select(p => p.nombre).ToList();
            }
            else
            {
                ViewBag.platos = null;
            }
            return View();
        }
    }
}
