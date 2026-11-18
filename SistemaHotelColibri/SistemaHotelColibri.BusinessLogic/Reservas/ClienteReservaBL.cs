using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Clientes;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reservas;
using SistemaHotelColibri.Abstract.Modelos.Clientes;

namespace SistemaHotelColibri.BusinessLogic.Reservas;

// La base exige un cliente en cada reserva y solo deja reservar a clientes activos.
// Por eso el cliente se deja Activo para registrar la reserva y, si se decidió no guardarlo,
// se oculta (Inactivo) al final, sin perder sus datos en la reserva.
public class ClienteReservaBL : IClienteReservaBL
{
    private readonly IBuscarClienteBL _buscarBL;
    private readonly IAgregarClienteBL _agregarBL;
    private readonly IEditarClienteBL _editarBL;
    private readonly IEliminarClienteBL _eliminarBL;
    private readonly IObtenerClientePorIdBL _obtenerPorIdBL;

    public ClienteReservaBL(
        IBuscarClienteBL buscarBL,
        IAgregarClienteBL agregarBL,
        IEditarClienteBL editarBL,
        IEliminarClienteBL eliminarBL,
        IObtenerClientePorIdBL obtenerPorIdBL)
    {
        _buscarBL = buscarBL;
        _agregarBL = agregarBL;
        _editarBL = editarBL;
        _eliminarBL = eliminarBL;
        _obtenerPorIdBL = obtenerPorIdBL;
    }

    public async Task<ResultadoOperacion> PrepararAsync(ClienteDto datos)
    {
        var identificacion = datos.Identificacion?.Trim() ?? string.Empty;
        if (identificacion.Length == 0)
        {
            return ResultadoOperacion.Error("Escribí la identificación del cliente.");
        }

        var existente = (await _buscarBL.BuscarAsync(identificacion, null))
            .FirstOrDefault(c => string.Equals(c.Identificacion, identificacion, StringComparison.OrdinalIgnoreCase));

        if (existente == null)
        {
            return await _agregarBL.AgregarAsync(datos);
        }

        if (existente.EstadoCliente == Estados.Cliente.Inactivo)
        {
            existente.EstadoCliente = Estados.Cliente.Activo;
            var activado = await _editarBL.EditarAsync(existente);
            if (!activado.Exito)
            {
                return activado;
            }
        }

        return ResultadoOperacion.Ok($"Se usó el cliente ya registrado {existente.NombreMostrar}.", existente.IdCliente);
    }

    public async Task FinalizarAsync(int idCliente, bool guardarEnClientes)
    {
        if (guardarEnClientes || idCliente <= 0)
        {
            return;
        }

        var cliente = await _obtenerPorIdBL.ObtenerPorIdAsync(idCliente);
        if (cliente?.EstadoCliente == Estados.Cliente.Activo)
        {
            await _eliminarBL.EliminarAsync(idCliente);
        }
    }
}
