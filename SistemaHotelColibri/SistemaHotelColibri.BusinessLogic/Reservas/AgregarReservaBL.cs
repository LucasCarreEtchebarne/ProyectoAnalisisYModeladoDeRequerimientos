using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reservas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;
using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.BusinessLogic.Reservas;

public class AgregarReservaBL : IAgregarReservaBL
{
    private readonly IAgregarReservaDA _da;
    private readonly ValidadorReserva _validador;

    public AgregarReservaBL(
        IAgregarReservaDA da,
        IObtenerClientePorIdDA clienteDA,
        IObtenerHabitacionPorIdDA habitacionDA,
        IHabitacionDisponibleReservaDA disponibleDA)
    {
        _da = da;
        _validador = new ValidadorReserva(clienteDA, habitacionDA, disponibleDA);
    }

    public async Task<ResultadoOperacion> AgregarAsync(ReservaDto reserva)
    {
        ValidadorReserva.Normalizar(reserva);
        var error = await _validador.ValidarAsync(reserva, null);
        if (error != null)
        {
            return ResultadoOperacion.Error(error);
        }

        try
        {
            var id = await _da.AgregarAsync(reserva);
            return ResultadoOperacion.Ok(Mensajes.ReservaRegistrada, id);
        }
        catch (Exception)
        {
            return ResultadoOperacion.Error(Mensajes.ErrorRegistrarReserva);
        }
    }
}
