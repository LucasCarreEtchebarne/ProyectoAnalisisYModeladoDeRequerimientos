using SistemaHotelColibri.ViewModels.Comun;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Controllers;

public abstract class ControladorBase : Controller
{
    protected int IdUsuarioActual =>
        int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : 0;

    // Tarjeta de resumen que filtra el listado por el estado indicado al hacer clic.
    protected TarjetaResumenViewModel TarjetaEstado(string titulo, int valor, string icono, string color,
        string estado, string? estadoSeleccionado, string? detalle = null) =>
        TarjetaResumenViewModel.Crear(titulo, valor, icono, color,
            Url.Action("Index", new { estado }), estado == estadoSeleccionado, detalle);

    protected void MensajeExito(string mensaje) => TempData["MensajeExito"] = mensaje;

    protected void MensajeError(string mensaje) => TempData["MensajeError"] = mensaje;

    protected void MostrarResultado(ResultadoOperacion resultado)
    {
        if (resultado.Exito)
        {
            MensajeExito(resultado.Mensaje);
        }
        else
        {
            MensajeError(resultado.Mensaje);
        }
    }
}
