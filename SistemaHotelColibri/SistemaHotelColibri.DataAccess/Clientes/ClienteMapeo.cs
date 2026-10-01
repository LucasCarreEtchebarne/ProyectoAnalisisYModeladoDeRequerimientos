using SistemaHotelColibri.Abstract.Modelos.Clientes;
using SistemaHotelColibri.DataAccess.Entidades;

namespace SistemaHotelColibri.DataAccess.Clientes;

public static class ClienteMapeo
{
    public static ClienteDto ADto(this Cliente entidad) => new()
    {
        IdCliente = entidad.IdCliente,
        Identificacion = entidad.Identificacion,
        NombreCompleto = entidad.NombreCompleto,
        PrimerApellido = entidad.PrimerApellido,
        SegundoApellido = entidad.SegundoApellido,
        Telefono = entidad.Telefono,
        CorreoElectronico = entidad.CorreoElectronico,
        Direccion = entidad.Direccion,
        FechaRegistro = entidad.FechaRegistro,
        EstadoCliente = entidad.EstadoCliente
    };

    public static Cliente AEntidad(this ClienteDto dto) => new()
    {
        IdCliente = dto.IdCliente,
        Identificacion = dto.Identificacion,
        NombreCompleto = dto.NombreCompleto,
        PrimerApellido = dto.PrimerApellido,
        SegundoApellido = dto.SegundoApellido,
        Telefono = dto.Telefono,
        CorreoElectronico = dto.CorreoElectronico,
        Direccion = dto.Direccion,
        FechaRegistro = dto.FechaRegistro,
        EstadoCliente = dto.EstadoCliente
    };
}