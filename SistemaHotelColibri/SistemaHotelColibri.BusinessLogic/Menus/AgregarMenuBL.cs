using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Menus;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Menus;
using SistemaHotelColibri.Abstract.Modelos.Menus;

namespace SistemaHotelColibri.BusinessLogic.Menus;

public class AgregarMenuBL : IAgregarMenuBL
{
    private readonly IAgregarMenuDA _agregarDA;
    private readonly IExisteNombreMenuDA _existeNombreDA;

    public AgregarMenuBL(IAgregarMenuDA agregarDA, IExisteNombreMenuDA existeNombreDA)
    {
        _agregarDA = agregarDA;
        _existeNombreDA = existeNombreDA;
    }

    public async Task<ResultadoOperacion> AgregarAsync(MenuDto producto)
    {
        ReglasMenu.Normalizar(producto);

        var error = ReglasMenu.Validar(producto);
        if (error != null)
        {
            return ResultadoOperacion.Error(error);
        }

        if (await _existeNombreDA.ExisteNombreAsync(producto.NombreProducto, 0))
        {
            return ResultadoOperacion.Error(Mensajes.MenuDuplicado);
        }

        producto.IdProductoMenu = 0;

        try
        {
            var idGenerado = await _agregarDA.AgregarAsync(producto);
            return ResultadoOperacion.Ok(Mensajes.MenuRegistrado, idGenerado);
        }
        catch (Exception)
        {
            return ResultadoOperacion.Error(Mensajes.ErrorRegistrarMenu);
        }
    }
}
