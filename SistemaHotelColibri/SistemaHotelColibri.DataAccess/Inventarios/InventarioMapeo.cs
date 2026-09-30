using SistemaHotelColibri.Abstract.Modelos.Inventarios;
using SistemaHotelColibri.DataAccess.Entidades;

namespace SistemaHotelColibri.DataAccess.Inventarios;

public static class InventarioMapeo
{
    public static InventarioDto ADto(this Inventario entidad) => new()
    {
        IdProducto = entidad.IdProducto,
        NombreProducto = entidad.NombreProducto,
        CategoriaProducto = entidad.CategoriaProducto,
        UnidadMedida = entidad.UnidadMedida,
        Stock = entidad.Stock,
        StockMinimo = entidad.StockMinimo,
        FechaIngreso = entidad.FechaIngreso,
        EstadoProducto = entidad.EstadoProducto,
        Descripcion = entidad.Descripcion
    };

    public static Inventario AEntidad(this InventarioDto dto) => new()
    {
        IdProducto = dto.IdProducto,
        NombreProducto = dto.NombreProducto,
        CategoriaProducto = dto.CategoriaProducto,
        UnidadMedida = dto.UnidadMedida,
        Stock = dto.Stock,
        StockMinimo = dto.StockMinimo,
        FechaIngreso = dto.FechaIngreso,
        EstadoProducto = dto.EstadoProducto,
        Descripcion = dto.Descripcion
    };
}
