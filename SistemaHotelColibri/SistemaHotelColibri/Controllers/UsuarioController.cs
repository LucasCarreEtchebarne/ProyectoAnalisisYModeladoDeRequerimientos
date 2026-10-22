using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;
using SistemaHotelColibri.Abstract.Modelos.Usuarios;
using SistemaHotelColibri.ViewModels.Comun;
using SistemaHotelColibri.ViewModels.Usuarios;

namespace SistemaHotelColibri.Controllers;

[Authorize(Roles = Roles.Administrador)]
public class UsuarioController : ControladorBase
{
    private const string TodosLosRoles = "Todos";

    private readonly IAgregarUsuarioBL _agregarBL;
    private readonly IObtenerUsuarioBL _obtenerBL;
    private readonly IObtenerUsuarioPorIdBL _obtenerPorIdBL;
    private readonly IEditarUsuarioBL _editarBL;
    private readonly IEliminarUsuarioBL _eliminarBL;
    private readonly IActivarUsuarioBL _activarBL;
    private readonly IBloquearUsuarioBL _bloquearBL;
    private readonly IRestablecerContrasenaUsuarioBL _restablecerBL;
    private readonly IObtenerRolBL _obtenerRolBL;

    public UsuarioController(
        IAgregarUsuarioBL agregarBL,
        IObtenerUsuarioBL obtenerBL,
        IObtenerUsuarioPorIdBL obtenerPorIdBL,
        IEditarUsuarioBL editarBL,
        IEliminarUsuarioBL eliminarBL,
        IActivarUsuarioBL activarBL,
        IBloquearUsuarioBL bloquearBL,
        IRestablecerContrasenaUsuarioBL restablecerBL,
        IObtenerRolBL obtenerRolBL)
    {
        _agregarBL = agregarBL;
        _obtenerBL = obtenerBL;
        _obtenerPorIdBL = obtenerPorIdBL;
        _editarBL = editarBL;
        _eliminarBL = eliminarBL;
        _activarBL = activarBL;
        _bloquearBL = bloquearBL;
        _restablecerBL = restablecerBL;
        _obtenerRolBL = obtenerRolBL;
    }

    private async Task CargarRolesAsync()
    {
        ViewBag.Roles = await _obtenerRolBL.ObtenerActivosAsync();
    }

    public async Task<IActionResult> Index(string? busqueda, string? estado, string? rol, int pagina = 1)
    {
        var estadoSeleccionado = estado ?? Estados.Usuario.Activo;
        var rolSeleccionado = rol ?? TodosLosRoles;

        var filtro = new FiltroUsuario
        {
            Busqueda = busqueda,
            Estado = estadoSeleccionado == FiltroListado.TodosLosEstados ? null : estadoSeleccionado,
            Rol = rolSeleccionado == TodosLosRoles ? null : rolSeleccionado,
            Pagina = pagina
        };

        var roles = (await _obtenerRolBL.ObtenerActivosAsync()).Select(r => r.Nombre);

        ViewBag.Busqueda = new BarraBusquedaViewModel
        {
            Busqueda = busqueda,
            Placeholder = "Nombre, usuario o correo",
            Filtros =
            [
                FiltroSeleccion.Crear("estado", "Estado",
                    Estados.Usuario.Filtro.Append(FiltroListado.TodosLosEstados), estadoSeleccionado),
                FiltroSeleccion.Crear("rol", "Rol", roles.Prepend(TodosLosRoles), rolSeleccionado)
            ]
        };
        ViewBag.IdUsuarioActual = IdUsuarioActual;
        return View(await _obtenerBL.ObtenerAsync(filtro));
    }

    public async Task<IActionResult> Detalle(int id)
    {
        var usuario = await _obtenerPorIdBL.ObtenerPorIdAsync(id);
        if (usuario == null)
        {
            MensajeError(Mensajes.NoEncontrado);
            return RedirectToAction(nameof(Index));
        }

        ViewBag.IdUsuarioActual = IdUsuarioActual;
        return View(usuario);
    }

    public async Task<IActionResult> Crear()
    {
        await CargarRolesAsync();
        return View(new UsuarioFormViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Crear(UsuarioFormViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            await CargarRolesAsync();
            return View(modelo);
        }

        var resultado = await _agregarBL.AgregarAsync(modelo.ADto(), modelo.Contrasena!);
        if (!resultado.Exito)
        {
            ModelState.AddModelError(string.Empty, resultado.Mensaje);
            await CargarRolesAsync();
            return View(modelo);
        }

        MensajeExito(resultado.Mensaje);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Editar(int id)
    {
        var usuario = await _obtenerPorIdBL.ObtenerPorIdAsync(id);
        if (usuario == null)
        {
            MensajeError(Mensajes.NoEncontrado);
            return RedirectToAction(nameof(Index));
        }

        if (usuario.Estado == Estados.Usuario.Inactivo)
        {
            MensajeError(Mensajes.UsuarioInactivoNoEditable);
            return RedirectToAction(nameof(Detalle), new { id });
        }

        await CargarRolesAsync();
        ViewBag.EsUsuarioActual = id == IdUsuarioActual;
        return View(UsuarioFormViewModel.DesdeDto(usuario));
    }

    [HttpPost]
    public async Task<IActionResult> Editar(int id, UsuarioFormViewModel modelo)
    {
        modelo.IdUsuario = id;
        ModelState.Remove(nameof(UsuarioFormViewModel.Contrasena));
        ModelState.Remove(nameof(UsuarioFormViewModel.ConfirmarContrasena));

        if (!ModelState.IsValid)
        {
            await CargarRolesAsync();
            ViewBag.EsUsuarioActual = id == IdUsuarioActual;
            return View(modelo);
        }

        var resultado = await _editarBL.EditarAsync(modelo.ADto(), IdUsuarioActual);
        if (!resultado.Exito)
        {
            ModelState.AddModelError(string.Empty, resultado.Mensaje);
            await CargarRolesAsync();
            ViewBag.EsUsuarioActual = id == IdUsuarioActual;
            return View(modelo);
        }

        MensajeExito(resultado.Mensaje);
        return RedirectToAction(nameof(Detalle), new { id });
    }

    [HttpPost]
    public async Task<IActionResult> Eliminar(int id)
    {
        MostrarResultado(await _eliminarBL.EliminarAsync(id, IdUsuarioActual));
        return RedirectToAction(nameof(Detalle), new { id });
    }

    [HttpPost]
    public async Task<IActionResult> Activar(int id)
    {
        MostrarResultado(await _activarBL.ActivarAsync(id));
        return RedirectToAction(nameof(Detalle), new { id });
    }

    [HttpPost]
    public async Task<IActionResult> Bloquear(int id)
    {
        MostrarResultado(await _bloquearBL.BloquearAsync(id, IdUsuarioActual));
        return RedirectToAction(nameof(Detalle), new { id });
    }

    [HttpPost]
    public async Task<IActionResult> Desbloquear(int id)
    {
        MostrarResultado(await _bloquearBL.DesbloquearAsync(id));
        return RedirectToAction(nameof(Detalle), new { id });
    }

    [HttpPost]
    public async Task<IActionResult> RestablecerContrasena(int id)
    {
        MostrarResultado(await _restablecerBL.RestablecerAsync(id));
        return RedirectToAction(nameof(Detalle), new { id });
    }
}
