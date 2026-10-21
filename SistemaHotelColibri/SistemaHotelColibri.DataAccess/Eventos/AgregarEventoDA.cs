using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Eventos;
using SistemaHotelColibri.Abstract.Modelos.Eventos;
using SistemaHotelColibri.DataAccess.Contexto;
using SistemaHotelColibri.DataAccess.Entidades;

namespace SistemaHotelColibri.DataAccess.Eventos;

public class AgregarEventoDA : IAgregarEventoDA
{
    private readonly HotelColibriContext _contexto;

    public AgregarEventoDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<int> AgregarAsync(EventoDto evento)
    {
        var entidad = new Evento
        {
            IdCliente = evento.IdCliente,
            IdEspacio = evento.IdEspacio,
            NombreEvento = evento.NombreEvento,
            FechaEvento = evento.FechaEvento,
            HoraInicio = evento.HoraInicio,
            HoraFin = evento.HoraFin,
            Participantes = evento.Participantes,
            MontoAcordado = evento.MontoAcordado,
            EstadoEvento = evento.EstadoEvento,
            Descripcion = evento.Descripcion
        };
        _contexto.Evento.Add(entidad);
        await _contexto.SaveChangesAsync();
        return entidad.IdEvento;
    }
}
