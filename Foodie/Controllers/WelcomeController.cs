using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Foodie.Controllers
{
    public class WelcomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Welcome()
        {
            return View();
        }
    }
}
