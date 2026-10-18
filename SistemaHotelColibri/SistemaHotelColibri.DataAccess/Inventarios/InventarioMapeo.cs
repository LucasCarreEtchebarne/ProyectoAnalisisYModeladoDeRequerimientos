using SistemaHotelColibri.Abstract.Modelos.Inventarios;
using SistemaHotelColibri.DataAccess.Entidades;

namespace SistemaHotelColibri.DataAccess.Inventarios;

public static class InventarioMapeo
{
    public static InventarioDto ADto(this Inventario entidad) => new()
    {
        IdProducto = entidad.IdProducto,
        NombreProducto = entidad.NombreProducto,
        IdCategoria = entidad.IdCategoria,
        NombreCategoria = entidad.IdCategoriaNavigation?.NombreCategoria ?? string.Empty,
        AreaCategoria = entidad.IdCategoriaNavigation?.Area ?? string.Empty,
        IdProveedor = entidad.IdProveedor,
        NombreProveedor = entidad.IdProveedorNavigation?.NombreProveedor,
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
        IdCategoria = dto.IdCategoria,
        IdProveedor = dto.IdProveedor,
        UnidadMedida = dto.UnidadMedida,
        Stock = dto.Stock,
        StockMinimo = dto.StockMinimo,
        FechaIngreso = dto.FechaIngreso,
        EstadoProducto = dto.EstadoProducto,
        Descripcion = dto.Descripcion
    };

    public static CategoriaInventarioDto ADto(this CategoriaInventario entidad) => new()
    {
        IdCategoria = entidad.IdCategoria,
        NombreCategoria = entidad.NombreCategoria,
        Area = entidad.Area,
        EstadoCategoria = entidad.EstadoCategoria,
        Descripcion = entidad.Descripcion
    };
}
