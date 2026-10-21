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
        CantidadCamas = entidad.CantidadCamas,
        TipoCama = entidad.TipoCama,
        BanoPrivado = entidad.BanoPrivado,
        Vista = entidad.Vista,
        AireAcondicionado = entidad.AireAcondicionado,
        Television = entidad.Television,
        Wifi = entidad.Wifi,
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
        CantidadCamas = dto.CantidadCamas,
        TipoCama = dto.TipoCama,
        BanoPrivado = dto.BanoPrivado,
        Vista = dto.Vista,
        AireAcondicionado = dto.AireAcondicionado,
        Television = dto.Television,
        Wifi = dto.Wifi,
        Descripcion = dto.Descripcion
    };
}
