using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Filtros;

public class RequiereCambioContrasenaFiltro : IActionFilter
{
    private static readonly string[] AccionesPermitidas = ["CambiarContrasena", "CerrarSesion"];

    public void OnActionExecuting(ActionExecutingContext contexto)
    {
        var usuario = contexto.HttpContext.User;
        if (usuario.Identity?.IsAuthenticated != true || !usuario.HasClaim(c => c.Type == Reclamos.DebeCambiarContrasena))
        {
            return;
        }

        var controlador = contexto.RouteData.Values["controller"]?.ToString();
        var accion = contexto.RouteData.Values["action"]?.ToString();
        if (controlador == "Cuenta" && AccionesPermitidas.Contains(accion))
        {
            return;
        }

        contexto.Result = new RedirectToActionResult("CambiarContrasena", "Cuenta", null);
    }

    public void OnActionExecuted(ActionExecutedContext contexto)
    {
    }
}
