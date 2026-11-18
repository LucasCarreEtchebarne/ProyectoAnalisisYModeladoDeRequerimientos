using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Clientes;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reservas;

// Cliente escrito directamente en el formulario de la reserva.
public interface IClienteReservaBL
{
    // Deja al cliente listo (Activo) para poder reservar: usa el que ya existe con esa
    // identificación o lo crea. IdGenerado es el id del cliente.
    Task<ResultadoOperacion> PrepararAsync(ClienteDto datos);

    // Después de intentar la reserva: si no se quiso guardar, el cliente queda Inactivo (oculto en Clientes).
    Task FinalizarAsync(int idCliente, bool guardarEnClientes);
}
