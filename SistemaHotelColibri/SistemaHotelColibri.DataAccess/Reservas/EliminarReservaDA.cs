using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Reservas;

public class EliminarReservaDA : IEliminarReservaDA
{
    private readonly HotelColibriContext _contexto;

    public EliminarReservaDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> EliminarAsync(int idReserva)
    {
        var entidad = await _contexto.Reserva.FindAsync(idReserva);
        if (entidad == null)
        {
            return false;
        }

        entidad.EstadoReserva = Estados.Reserva.Cancelada;
        await _contexto.SaveChangesAsync();
        return true;
    }
}
