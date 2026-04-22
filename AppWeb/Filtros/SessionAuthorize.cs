using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppWeb.Filtros
{
    public class SessionAuthorize : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var usuario = context.HttpContext.Session.GetString("usuario");
            if (usuario == null)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }

    }
}
