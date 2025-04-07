using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Foodie.Models;
using Foodie.Servicios;
using Microsoft.AspNetCore.Authorization;

namespace Foodie.Controllers;

public class HomeController : Controller
{
    private readonly FoodieContext foodieContext_;

    public HomeController(FoodieContext context)
    {
        foodieContext_ = context;
    }

    [Autenticacion]
    public async Task<IActionResult> Index()
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

        var platos =  foodieContext_.Platos
            .Where(p => p.estado == 1)
            .ToList();

        if (platos != null)
        {
            ViewBag.platosImagenes = platos.Select(p => p.imagen).ToList();
        }
        else
        {
            ViewBag.platos = null;
        }

        return View();  
    }

    [Autenticacion]
    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Exit()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Autenticar", "Login_Clientes");
    }
   

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
