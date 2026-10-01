using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Clientes;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.Abstract.Modelos.Clientes;

namespace SistemaHotelColibri.BusinessLogic.Clientes;

public class EditarClienteBL : IEditarClienteBL
{
    private readonly IEditarClienteDA _editarDA;
    private readonly IObtenerClientePorIdDA _obtenerPorIdDA;
    private readonly IExisteIdentificacionClienteDA _existeIdentificacionDA;

    public EditarClienteBL(
        IEditarClienteDA editarDA,
        IObtenerClientePorIdDA obtenerPorIdDA,
        IExisteIdentificacionClienteDA existeIdentificacionDA)
    {
        _editarDA = editarDA;
        _obtenerPorIdDA = obtenerPorIdDA;
        _existeIdentificacionDA = existeIdentificacionDA;
    }

    public async Task<ResultadoOperacion> EditarAsync(ClienteDto datos)
    {
        var cliente = await _obtenerPorIdDA.ObtenerPorIdAsync(datos.IdCliente);
        if (cliente == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        ReglasCliente.Normalizar(datos);

        var error = ReglasCliente.Validar(datos);
        if (error != null)
        {
            return ResultadoOperacion.Error(error);
        }

        if (await _existeIdentificacionDA.ExisteIdentificacionAsync(datos.Identificacion, datos.IdCliente))
        {
            return ResultadoOperacion.Error(Mensajes.ClienteDuplicado);
        }

        // La fecha de registro se conserva; solo se actualizan los datos editables.
        cliente.Identificacion = datos.Identificacion;
        cliente.NombreCompleto = datos.NombreCompleto;
        cliente.PrimerApellido = datos.PrimerApellido;
        cliente.SegundoApellido = datos.SegundoApellido;
        cliente.Telefono = datos.Telefono;
        cliente.CorreoElectronico = datos.CorreoElectronico;
        cliente.Direccion = datos.Direccion;
        cliente.EstadoCliente = datos.EstadoCliente;

        if (!await _editarDA.EditarAsync(cliente))
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        return ResultadoOperacion.Ok(Mensajes.ClienteModificado, cliente.IdCliente);
    }
}