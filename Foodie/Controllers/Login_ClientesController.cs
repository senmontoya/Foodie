using Microsoft.AspNetCore.Mvc;
using Foodie.Models;
using Microsoft.EntityFrameworkCore;
using Foodie.Servicios;

namespace Foodie.Controllers
{
    public class Login_ClientesController : Controller
    {
        private readonly FoodieContext foodieContext_;

        public Login_ClientesController(FoodieContext context)
        {
            foodieContext_ = context;
        }

        [Autenticacion]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Autenticar()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Autenticar(string correo, string password)
        {
            var authenticatedUser = await (from ll in foodieContext_.Login_Cliente
                                     where ll.correo == correo
                                     && ll.contraseña == password
                                     select ll).FirstOrDefaultAsync();

            if (authenticatedUser != null)
            {
                HttpContext.Session.SetInt32("loginid", authenticatedUser.loginid);
                HttpContext.Session.SetString("correo", authenticatedUser.correo);
                HttpContext.Session.SetString("password", authenticatedUser.contraseña);
                return RedirectToAction("Index", "Login_Clientes");
            }

            ViewData["ErrorMessage"] = "Usuario o contraseña incorrectos";
            return View();
        }



    }
}

    


