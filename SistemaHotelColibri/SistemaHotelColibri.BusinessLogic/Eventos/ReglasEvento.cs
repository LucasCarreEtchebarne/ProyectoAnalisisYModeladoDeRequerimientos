using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Clientes;
using SistemaHotelColibri.Abstract.Modelos.Eventos;

namespace SistemaHotelColibri.BusinessLogic.Eventos;

internal static class ReglasEvento
{
    public static void Normalizar(EventoDto evento)
    {
        evento.NombreEvento = (evento.NombreEvento ?? string.Empty).Trim();
        evento.Descripcion = string.IsNullOrWhiteSpace(evento.Descripcion) ? null : evento.Descripcion.Trim();
    }

    public static string? Validar(EventoDto evento, EventoDto? original, ClienteDto? cliente, EspacioEventoDto? espacio)
    {
        if (evento.IdCliente <= 0 || evento.IdEspacio <= 0
            || string.IsNullOrWhiteSpace(evento.NombreEvento)
            || evento.FechaEvento == default
            || string.IsNullOrWhiteSpace(evento.EstadoEvento))
        {
            return Mensajes.EventoCamposIncompletos;
        }

        if (!Estados.Evento.Todos.Contains(evento.EstadoEvento))
        {
            return Mensajes.EstadoEventoInvalido;
        }

        if (evento.HoraFin <= evento.HoraInicio)
        {
            return Mensajes.EventoHorasInvalidas;
        }

        if (evento.Participantes <= 0)
        {
            return Mensajes.ParticipantesInvalidos;
        }

        if (evento.MontoAcordado < 0)
        {
            return Mensajes.MontoAcordadoInvalido;
        }

        var fechaCambio = original == null
            || original.FechaEvento != evento.FechaEvento
            || original.IdEspacio != evento.IdEspacio;
        if (fechaCambio && evento.FechaEvento < DateOnly.FromDateTime(DateTime.Today))
        {
            return Mensajes.EventoFechaPasada;
        }

        if (original != null
            && evento.EstadoEvento == Estados.Evento.Cancelado
            && original.EstadoEvento == Estados.Evento.Realizado)
        {
            return Mensajes.EventoYaRealizado;
        }

        if (cliente == null)
        {
            return Mensajes.ClienteNoRegistrado;
        }

        if (cliente.EstadoCliente == Estados.Cliente.Inactivo && (original == null || original.IdCliente != evento.IdCliente))
        {
            return Mensajes.ClienteReservaInactivo;
        }

        if (espacio == null)
        {
            return Mensajes.NoEncontrado;
        }

        if (espacio.EstadoEspacio == Estados.EspacioEvento.Inactivo && (original == null || original.IdEspacio != evento.IdEspacio))
        {
            return Mensajes.EspacioEventoInactivo;
        }

        if (evento.Participantes > espacio.CapacidadMaxima)
        {
            return string.Format(Mensajes.ParticipantesExcedenCapacidad, espacio.CapacidadMaxima);
        }

        return null;
    }
}
