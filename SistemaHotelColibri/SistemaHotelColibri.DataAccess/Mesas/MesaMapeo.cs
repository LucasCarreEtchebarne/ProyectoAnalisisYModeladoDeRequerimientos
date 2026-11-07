using SistemaHotelColibri.Abstract.Modelos.Mesas;
using SistemaHotelColibri.DataAccess.Entidades;

namespace SistemaHotelColibri.DataAccess.Mesas;

public static class MesaMapeo
{
    public static MesaDto ADto(this Mesa entidad) => new()
    {
        IdMesa = entidad.IdMesa,
        NumeroMesa = entidad.NumeroMesa,
        Capacidad = entidad.Capacidad,
        EstadoMesa = entidad.EstadoMesa,
        Descripcion = entidad.Descripcion
    };

    public static Mesa AEntidad(this MesaDto dto) => new()
    {
        IdMesa = dto.IdMesa,
        NumeroMesa = dto.NumeroMesa,
        Capacidad = dto.Capacidad,
        EstadoMesa = dto.EstadoMesa,
        Descripcion = dto.Descripcion
    };
}
