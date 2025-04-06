using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Foodie.Models;
using Foodie.Servicios;

namespace Foodie.Controllers;

public class HomeController : Controller
{
    private readonly FoodieContext foodieContext_;

    public HomeController(FoodieContext context)
    {
        foodieContext_ = context;
    }

    [Autenticacion]
    public IActionResult Index()
    {
        var loginid = HttpContext.Session.GetInt32("loginid");
        var correo = HttpContext.Session.GetString("correo");
        var password = HttpContext.Session.GetString("password");

        if (loginid == null)
        {
            return RedirectToAction("Autenticar", "Login_Clientes");
        }

        var cliente = foodieContext_.Cliente.FirstOrDefault(c => c.login_Cliente.loginid == loginid);

        if (cliente != null)
        {
            ViewBag.nombre = cliente.nombre;
            ViewBag.ClienteId = cliente.clienteId;
            HttpContext.Session.SetInt32("clienteId", cliente.clienteId);
        }
        else
        {
            ViewBag.Nombre = "Invitado";
            ViewBag.ClienteId = 0;
        }

        ViewBag.correo = correo;
        return View();  
    }
    [Autenticacion]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
