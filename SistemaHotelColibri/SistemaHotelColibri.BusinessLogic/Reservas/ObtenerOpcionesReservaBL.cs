using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reservas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;
using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.BusinessLogic.Reservas;

public class ObtenerOpcionesReservaBL : IObtenerOpcionesReservaBL
{
    private readonly IObtenerOpcionesReservaDA _obtenerDA;

    public ObtenerOpcionesReservaBL(IObtenerOpcionesReservaDA obtenerDA)
    {
        _obtenerDA = obtenerDA;
    }

    public Task<OpcionesReservaDto> ObtenerAsync(int idClienteActual = 0, int idHabitacionActual = 0) =>
        _obtenerDA.ObtenerAsync(idClienteActual, idHabitacionActual);
}
