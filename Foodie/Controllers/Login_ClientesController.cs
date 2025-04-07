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
        public IActionResult Registrarse()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registrarse(string correo, string contraseña, string nombre, string telefono, string direccion)
        {
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contraseña) ||
                string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(telefono) ||
                string.IsNullOrEmpty(direccion))
            {
                ViewData["ErrorMessage"] = "Todos los campos son obligatorios.";
                return View();
            }

            var existingUser = await foodieContext_.Login_Cliente
                .FirstOrDefaultAsync(lc => lc.correo == correo);

            if (existingUser != null)
            {
                ViewData["ErrorMessage"] = "El correo ya está registrado.";
                return View();
            }

            var nuevoLogin = new Login_Cliente
            {
                correo = correo,
                contraseña = contraseña
            };
            foodieContext_.Login_Cliente.Add(nuevoLogin);
            await foodieContext_.SaveChangesAsync();

            var nuevoCliente = new Cliente
            {
                nombre = nombre,
                telefono = telefono,
                direccion = direccion,
                latitud = 13.123456M,
                longitud = -89.123456M,
                loginid = nuevoLogin.loginid // Usamos el loginid generado
            };
            foodieContext_.Cliente.Add(nuevoCliente);
            await foodieContext_.SaveChangesAsync();

            ViewData["SuccessMessage"] = "Registro exitoso. Por favor, inicia sesión.";
            return View();
        }
    }
}