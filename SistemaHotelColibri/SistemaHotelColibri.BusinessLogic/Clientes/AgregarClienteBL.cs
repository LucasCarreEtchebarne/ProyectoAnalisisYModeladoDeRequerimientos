using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Clientes;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.Abstract.Modelos.Clientes;

namespace SistemaHotelColibri.BusinessLogic.Clientes;

public class AgregarClienteBL : IAgregarClienteBL
{
    private readonly IAgregarClienteDA _agregarDA;
    private readonly IExisteIdentificacionClienteDA _existeIdentificacionDA;
    private readonly IExisteCorreoClienteDA _existeCorreoDA;

    public AgregarClienteBL(
        IAgregarClienteDA agregarDA,
        IExisteIdentificacionClienteDA existeIdentificacionDA,
        IExisteCorreoClienteDA existeCorreoDA)
    {
        _agregarDA = agregarDA;
        _existeIdentificacionDA = existeIdentificacionDA;
        _existeCorreoDA = existeCorreoDA;
    }

    public async Task<ResultadoOperacion> AgregarAsync(ClienteDto cliente)
    {
        cliente.IdCliente = 0;
        cliente.EstadoCliente = Estados.Cliente.Activo;
        cliente.FechaRegistro = DateTime.Now;

        ReglasCliente.Normalizar(cliente);

        var error = ReglasCliente.Validar(cliente);
        if (error != null)
        {
            return ResultadoOperacion.Error(error);
        }

        if (await _existeIdentificacionDA.ExisteIdentificacionAsync(cliente.Identificacion, 0))
        {
            return ResultadoOperacion.Error(Mensajes.ClienteDuplicado);
        }

        if (cliente.CorreoElectronico != null
            && await _existeCorreoDA.ExisteCorreoAsync(cliente.CorreoElectronico, 0))
        {
            return ResultadoOperacion.Error(Mensajes.CorreoClienteDuplicado);
        }

        var idGenerado = await _agregarDA.AgregarAsync(cliente);

        return ResultadoOperacion.Ok(string.Format(Mensajes.ClienteRegistrado, idGenerado), idGenerado);
    }
}