using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Foodie.Servicios
{
    public class AutenticacionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var loginid = context.HttpContext.Session.GetInt32("loginid");
            if (loginid == null)
            {
                context.Result = new RedirectToActionResult("Autenticar", "Login_Clientes", null);
            }
            base.OnActionExecuting(context);
        }
    }
    
}
