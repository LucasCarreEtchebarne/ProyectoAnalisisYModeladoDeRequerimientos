using Microsoft.AspNetCore.Mvc;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Inventarios;
using SistemaHotelColibri.ViewModels.Inventarios;

namespace SistemaHotelColibri.Controllers;

public class InventarioController : ControladorBase
{
    private readonly IAgregarInventarioBL _agregarBL;

    public InventarioController(IAgregarInventarioBL agregarBL)
    {
        _agregarBL = agregarBL;
    }

    public IActionResult Crear()
    {
        ViewBag.UnidadesMedida = Catalogos.UnidadesMedida;
        return View(new InventarioFormViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Crear(InventarioFormViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.UnidadesMedida = Catalogos.UnidadesMedida;
            return View(modelo);
        }

        var resultado = await _agregarBL.AgregarAsync(modelo.ADto());
        if (!resultado.Exito)
        {
            ModelState.AddModelError(string.Empty, resultado.Mensaje);
            ViewBag.UnidadesMedida = Catalogos.UnidadesMedida;
            return View(modelo);
        }

        MensajeExito(resultado.Mensaje);
        return RedirectToAction(nameof(Crear));
    }
}
