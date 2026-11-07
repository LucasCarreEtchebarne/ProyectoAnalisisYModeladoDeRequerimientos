using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Mesas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Mesas;
using SistemaHotelColibri.Abstract.Modelos.Mesas;

namespace SistemaHotelColibri.BusinessLogic.Mesas;

public class EditarMesaBL : IEditarMesaBL
{
    private readonly IEditarMesaDA _editarDA;
    private readonly IObtenerMesaPorIdDA _obtenerPorIdDA;
    private readonly IExisteNumeroMesaDA _existeNumeroDA;
    private readonly ITienePedidoActivoMesaDA _pedidoActivoDA;

    public EditarMesaBL(
        IEditarMesaDA editarDA,
        IObtenerMesaPorIdDA obtenerPorIdDA,
        IExisteNumeroMesaDA existeNumeroDA,
        ITienePedidoActivoMesaDA pedidoActivoDA)
    {
        _editarDA = editarDA;
        _obtenerPorIdDA = obtenerPorIdDA;
        _existeNumeroDA = existeNumeroDA;
        _pedidoActivoDA = pedidoActivoDA;
    }

    public async Task<ResultadoOperacion> EditarAsync(MesaDto datos)
    {
        var mesa = await _obtenerPorIdDA.ObtenerPorIdAsync(datos.IdMesa);
        if (mesa == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        ReglasMesa.Normalizar(datos);

        var error = ReglasMesa.Validar(datos);
        if (error != null)
        {
            return ResultadoOperacion.Error(error);
        }

        if (mesa.EstadoMesa == Estados.Mesa.Ocupada && datos.NumeroMesa != mesa.NumeroMesa)
        {
            return ResultadoOperacion.Error(Mensajes.MesaOcupadaNoModificable);
        }

        if (datos.EstadoMesa == Estados.Mesa.Inactiva
            && mesa.EstadoMesa != Estados.Mesa.Inactiva
            && await _pedidoActivoDA.TienePedidoActivoAsync(datos.IdMesa))
        {
            return ResultadoOperacion.Error(Mensajes.MesaConPedidoActivo);
        }

        var numeroDuplicado = await _existeNumeroDA.ExisteNumeroAsync(datos.NumeroMesa, datos.IdMesa);
        if (numeroDuplicado)
        {
            return ResultadoOperacion.Error(Mensajes.MesaDuplicada);
        }

        mesa.NumeroMesa = datos.NumeroMesa;
        mesa.Capacidad = datos.Capacidad;
        mesa.EstadoMesa = datos.EstadoMesa;
        mesa.Descripcion = datos.Descripcion;

        if (!await _editarDA.EditarAsync(mesa))
        {
            return ResultadoOperacion.Error(Mensajes.ErrorModificarMesa);
        }

        return ResultadoOperacion.Ok(Mensajes.MesaModificada, mesa.IdMesa);
    }
}
