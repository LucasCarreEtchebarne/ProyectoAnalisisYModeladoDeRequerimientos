using SistemaHotelColibri.Abstract.Modelos.Eventos;
using SistemaHotelColibri.DataAccess.Entidades;

namespace SistemaHotelColibri.DataAccess.Eventos;

internal static class EventoMapeo
{
    public static EventoDto ADto(this Evento e) => new()
    {
        IdEvento = e.IdEvento,
        IdCliente = e.IdCliente,
        IdEspacio = e.IdEspacio,
        NombreEvento = e.NombreEvento,
        FechaEvento = e.FechaEvento,
        HoraInicio = e.HoraInicio,
        HoraFin = e.HoraFin,
        Participantes = e.Participantes,
        MontoAcordado = e.MontoAcordado,
        EstadoEvento = e.EstadoEvento,
        Descripcion = e.Descripcion,
        NombreCliente = e.IdClienteNavigation == null ? string.Empty
            : $"{e.IdClienteNavigation.Nombre} {e.IdClienteNavigation.PrimerApellido} {e.IdClienteNavigation.SegundoApellido}".Trim(),
        IdentificacionCliente = e.IdClienteNavigation?.Identificacion ?? string.Empty,
        NombreEspacio = e.IdEspacioNavigation?.NombreEspacio ?? string.Empty,
        CapacidadEspacio = e.IdEspacioNavigation?.CapacidadMaxima ?? 0
    };

    public static EspacioEventoDto ADto(this EspacioEvento entidad) => new()
    {
        IdEspacio = entidad.IdEspacio,
        NombreEspacio = entidad.NombreEspacio,
        CapacidadMaxima = entidad.CapacidadMaxima,
        EstadoEspacio = entidad.EstadoEspacio
    };
}
