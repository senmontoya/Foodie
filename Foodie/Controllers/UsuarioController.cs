using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Foodie.Models;
using ProyectoVentas.Models.Dtos;

namespace Foodie.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly FoodieContext _context;

        public UsuarioController(FoodieContext context)
        {
            _context = context;
        }

        public IActionResult Perfil()
        {
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

            var login_cliente = _context.Login_Cliente.FirstOrDefault(lc => lc.loginid == cliente.loginid);

            if (login_cliente == null)
            {
                return RedirectToAction("Autenticar", "Login_Clientes");
            }

            ClienteViewModel clienteDto = new ClienteViewModel
            {
                Cliente = cliente,
                Login_Cliente = login_cliente
            };

            return View(clienteDto);
        }

        public IActionResult EditarDatosPersonales()
        {
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

            return View(cliente);
        }

        [HttpPost]
        public IActionResult EditarDatosPersonales(string nombre, string telefono, string direccion)
        {
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

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(telefono) || string.IsNullOrEmpty(direccion))
            {
                ViewBag.Error = "Todod los campos son obligatorios.";
                return View(cliente);
            }

            cliente.nombre = nombre;
            cliente.telefono = telefono;
            cliente.direccion = direccion;

            _context.Cliente.Entry(cliente).State = EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction("Perfil");
        }

        public IActionResult EditarInicioSesion()
        {
            var loginid = HttpContext.Session.GetInt32("loginid");

            if (loginid == null)
            {
                return RedirectToAction("Autenticar", "Login_Clientes");
            }

            var login_cliente = _context.Login_Cliente.FirstOrDefault(lc => lc.loginid == loginid);
            if (login_cliente == null)
            {
                return RedirectToAction("Autenticar", "Login_Clientes");
            }

            EditarInicioSesionViewModel editarInicioSesionDto = new EditarInicioSesionViewModel
            {
                loginId = login_cliente.loginid,
                correo = login_cliente.correo
            };

            return View(editarInicioSesionDto);
        }

        [HttpPost]
        public IActionResult EditarInicioSesion(EditarInicioSesionViewModel model)
        {
            var loginid = HttpContext.Session.GetInt32("loginid");

            if (loginid == null)
            {
                return RedirectToAction("Autenticar", "Login_Clientes");
            }

            var login_cliente = _context.Login_Cliente.FirstOrDefault(lc => lc.loginid == loginid);

            if (string.IsNullOrEmpty(model.correo) || string.IsNullOrEmpty(model.contrasenaActual) || string.IsNullOrEmpty(model.contrasenaNueva) || string.IsNullOrEmpty(model.contrasenaNuevaConfirmacion))
            {
                ViewBag.Error = "Todod los campos son obligatorios.";
                return View(model);
            }

            if(!login_cliente.contraseña.Equals(model.contrasenaActual))
            {
                ViewBag.Error = "Contraseña actual incorrecta.";
                return View(model);
            }

            if(!model.contrasenaNueva.Equals(model.contrasenaNuevaConfirmacion))
            {
                ViewBag.Error = "La contraseña no coincide con la confirmacion de contraseña.";
                return View(model);
            }

            login_cliente.correo = model.correo;
            login_cliente.contraseña = model.contrasenaNueva;

            _context.Login_Cliente.Entry(login_cliente).State = EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction("Perfil");
        }
    }
}
