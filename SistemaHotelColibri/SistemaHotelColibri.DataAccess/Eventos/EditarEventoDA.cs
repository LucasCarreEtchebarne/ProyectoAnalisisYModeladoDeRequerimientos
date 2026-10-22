using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Eventos;
using SistemaHotelColibri.Abstract.Modelos.Eventos;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Eventos;

public class EditarEventoDA : IEditarEventoDA
{
    private readonly HotelColibriContext _contexto;

    public EditarEventoDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> EditarAsync(EventoDto evento)
    {
        var entidad = await _contexto.Evento.FindAsync(evento.IdEvento);
        if (entidad == null)
        {
            return false;
        }

        entidad.IdCliente = evento.IdCliente;
        entidad.IdEspacio = evento.IdEspacio;
        entidad.NombreEvento = evento.NombreEvento;
        entidad.FechaEvento = evento.FechaEvento;
        entidad.HoraInicio = evento.HoraInicio;
        entidad.HoraFin = evento.HoraFin;
        entidad.Participantes = evento.Participantes;
        entidad.MontoAcordado = evento.MontoAcordado;
        entidad.EstadoEvento = evento.EstadoEvento;
        entidad.Descripcion = evento.Descripcion;

        await _contexto.SaveChangesAsync();
        return true;
    }
}
