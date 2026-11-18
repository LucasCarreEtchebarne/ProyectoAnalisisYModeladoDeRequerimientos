using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Pedidos;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;

namespace SistemaHotelColibri.BusinessLogic.Habitaciones;

// Una habitación no se puede inactivar si todavía tiene reservas vigentes o una cuenta abierta,
// porque desaparecería de Pedidos y de las reservas pero seguiría comprometida.
internal static class UsoHabitacion
{
    public static async Task<string?> MotivoParaNoInactivarAsync(
        int idHabitacion,
        IObtenerReservasVigentesDA reservasDA,
        IObtenerPedidosAbiertosDA pedidosDA)
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        var reserva = (await reservasDA.ObtenerAsync(hoy)).FirstOrDefault(r => r.IdHabitacion == idHabitacion);
        if (reserva != null)
        {
            return $"No se puede inactivar: tiene la reserva #{reserva.IdReserva} de {reserva.NombreCliente} " +
                   $"({reserva.FechaEntrada:dd/MM} al {reserva.FechaSalida:dd/MM}). Cancelala o cambiala de habitación primero.";
        }

        var cuenta = (await pedidosDA.ObtenerAsync()).FirstOrDefault(p => p.IdHabitacion == idHabitacion);
        if (cuenta != null)
        {
            return $"No se puede inactivar: tiene la cuenta abierta del pedido #{cuenta.IdPedido}. Cobrala o cerrala primero.";
        }

        return null;
    }
}
