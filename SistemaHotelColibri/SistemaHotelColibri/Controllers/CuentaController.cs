using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;
using SistemaHotelColibri.Abstract.Modelos.Usuarios;
using SistemaHotelColibri.ViewModels.Cuenta;

namespace SistemaHotelColibri.Controllers;

public class CuentaController : ControladorBase
{
    private readonly IIniciarSesionBL _iniciarSesionBL;
    private readonly ICerrarSesionBL _cerrarSesionBL;
    private readonly ICambiarContrasenaBL _cambiarContrasenaBL;
    private readonly IRecuperarContrasenaBL _recuperarContrasenaBL;
    private readonly ILogger<CuentaController> _registro;

    public CuentaController(
        IIniciarSesionBL iniciarSesionBL,
        ICerrarSesionBL cerrarSesionBL,
        ICambiarContrasenaBL cambiarContrasenaBL,
        IRecuperarContrasenaBL recuperarContrasenaBL,
        ILogger<CuentaController> registro)
    {
        _iniciarSesionBL = iniciarSesionBL;
        _cerrarSesionBL = cerrarSesionBL;
        _cambiarContrasenaBL = cambiarContrasenaBL;
        _recuperarContrasenaBL = recuperarContrasenaBL;
        _registro = registro;
    }

    [AllowAnonymous]
    public IActionResult IniciarSesion(string? returnUrl)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }

        return View(new IniciarSesionViewModel { ReturnUrl = returnUrl });
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> IniciarSesion(IniciarSesionViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        var resultado = await _iniciarSesionBL.IniciarSesionAsync(modelo.UsuarioOCorreo!, modelo.Contrasena!, modelo.Recordarme);

        switch (resultado)
        {
            case ResultadoInicioSesion.Exitoso:
                return Url.IsLocalUrl(modelo.ReturnUrl)
                    ? LocalRedirect(modelo.ReturnUrl)
                    : RedirectToAction("Index", "Home");

            case ResultadoInicioSesion.DebeCambiarContrasena:
                MensajeError(Mensajes.DebeCambiarContrasena);
                return RedirectToAction(nameof(CambiarContrasena));

            case ResultadoInicioSesion.Bloqueado:
                ModelState.AddModelError(string.Empty, Mensajes.UsuarioBloqueadoInicio);
                break;

            case ResultadoInicioSesion.Inactivo:
                ModelState.AddModelError(string.Empty, Mensajes.UsuarioInactivo);
                break;

            default:
                ModelState.AddModelError(string.Empty, Mensajes.CredencialesInvalidas);
                break;
        }

        modelo.Contrasena = null;
        return View(modelo);
    }

    [HttpPost]
    public async Task<IActionResult> CerrarSesion()
    {
        await _cerrarSesionBL.CerrarSesionAsync();
        MensajeExito(Mensajes.SesionCerrada);
        return RedirectToAction(nameof(IniciarSesion));
    }

    public IActionResult CambiarContrasena()
    {
        return View(new CambiarContrasenaViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> CambiarContrasena(CambiarContrasenaViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        var resultado = await _cambiarContrasenaBL.CambiarAsync(IdUsuarioActual, modelo.ContrasenaActual!, modelo.ContrasenaNueva!);
        if (!resultado.Exito)
        {
            ModelState.AddModelError(string.Empty, resultado.Mensaje);
            return View(new CambiarContrasenaViewModel());
        }

        MensajeExito(resultado.Mensaje);
        return RedirectToAction("Index", "Home");
    }

    [AllowAnonymous]
    public IActionResult OlvideContrasena()
    {
        return View(new OlvideContrasenaViewModel());
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> OlvideContrasena(OlvideContrasenaViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        var urlRestablecer = Url.Action(nameof(RestablecerContrasena), "Cuenta", null, Request.Scheme)!;

        try
        {
            await _recuperarContrasenaBL.SolicitarAsync(modelo.CorreoElectronico!, urlRestablecer);
        }
        catch (Exception ex)
        {
            _registro.LogError(ex, "Error al enviar el correo de recuperación de contraseña.");
            ModelState.AddModelError(string.Empty, Mensajes.ErrorEnvioCorreo);
            return View(modelo);
        }

        MensajeExito(Mensajes.RecuperacionEnviada);
        return RedirectToAction(nameof(IniciarSesion));
    }

    [AllowAnonymous]
    public IActionResult RestablecerContrasena(string? correo, string? token)
    {
        if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(token))
        {
            MensajeError(Mensajes.EnlaceRecuperacionInvalido);
            return RedirectToAction(nameof(IniciarSesion));
        }

        return View(new RestablecerContrasenaViewModel { Correo = correo, Token = token });
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> RestablecerContrasena(RestablecerContrasenaViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        var resultado = await _recuperarContrasenaBL.RestablecerAsync(modelo.Correo!, modelo.Token!, modelo.ContrasenaNueva!);
        if (!resultado.Exito)
        {
            ModelState.AddModelError(string.Empty, resultado.Mensaje);
            return View(modelo);
        }

        MensajeExito(resultado.Mensaje);
        return RedirectToAction(nameof(IniciarSesion));
    }

    [AllowAnonymous]
    public IActionResult AccesoDenegado()
    {
        return View();
    }
}
