using Microsoft.AspNetCore.Mvc;
using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Controllers;

public abstract class ControladorBase : Controller
{
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
