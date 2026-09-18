using System.Web.Mvc;
using System.Web.Routing;

namespace Prototipo.Infrastructure
{
    public class AutorizarAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var usuario = filterContext.HttpContext.User;

            if (usuario != null && usuario.Identity != null && usuario.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Error", action = "AccesoDenegado" }));
                return;
            }

            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}
