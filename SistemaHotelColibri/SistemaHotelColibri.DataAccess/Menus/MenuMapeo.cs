using SistemaHotelColibri.Abstract.Modelos.Menus;
using SistemaHotelColibri.DataAccess.Entidades;

namespace SistemaHotelColibri.DataAccess.Menus;

public static class MenuMapeo
{
    public static MenuDto ADto(this Menu entidad) => new()
    {
        IdProductoMenu = entidad.IdProductoMenu,
        NombreProducto = entidad.NombreProducto,
        CategoriaMenu = entidad.CategoriaMenu,
        Precio = entidad.Precio,
        EstadoProductoMenu = entidad.EstadoProductoMenu,
        Descripcion = entidad.Descripcion
    };

    public static Menu AEntidad(this MenuDto dto) => new()
    {
        IdProductoMenu = dto.IdProductoMenu,
        NombreProducto = dto.NombreProducto,
        CategoriaMenu = dto.CategoriaMenu,
        Precio = dto.Precio,
        EstadoProductoMenu = dto.EstadoProductoMenu,
        Descripcion = dto.Descripcion
    };
}
