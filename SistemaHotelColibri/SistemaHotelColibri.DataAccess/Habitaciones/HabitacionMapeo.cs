using SistemaHotelColibri.Abstract.Modelos.Habitaciones;
using SistemaHotelColibri.DataAccess.Entidades;

namespace SistemaHotelColibri.DataAccess.Habitaciones;

public static class HabitacionMapeo
{
    public static HabitacionDto ADto(this Habitacion entidad) => new()
    {
        IdHabitacion = entidad.IdHabitacion,
        NumeroHabitacion = entidad.NumeroHabitacion,
        TipoHabitacion = entidad.TipoHabitacion,
        Capacidad = entidad.Capacidad,
        Precio = entidad.Precio,
        EstadoHabitacion = entidad.EstadoHabitacion,
        Piso = entidad.Piso,
        Descripcion = entidad.Descripcion
    };

    public static Habitacion AEntidad(this HabitacionDto dto) => new()
    {
        IdHabitacion = dto.IdHabitacion,
        NumeroHabitacion = dto.NumeroHabitacion,
        TipoHabitacion = dto.TipoHabitacion,
        Capacidad = dto.Capacidad,
        Precio = dto.Precio,
        EstadoHabitacion = dto.EstadoHabitacion,
        Piso = dto.Piso,
        Descripcion = dto.Descripcion
    };
}
